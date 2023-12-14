//
// Created by dimkolya on 13.09.22.
//

#include "huffman_decoder_tree.h"

huffman_decoder_tree::node::node(unsigned char c)
    : c(c), zero(nullptr), one(nullptr), buffered_chars(), buffered_way() {
  for (std::size_t i = 0; i < CHAR_BIT; ++i) {
    buffered_way[i] = nullptr;
  }
}

huffman_decoder_tree::node::node(node* zero, node* one)
    : c('\0'), zero(zero), one(one), buffered_chars(), buffered_way() {
  for (std::size_t i = 0; i < CHAR_BIT; ++i) {
    buffered_way[i] = nullptr;
  }
}

huffman_decoder_tree::node::~node() = default;

bool huffman_decoder_tree::node::is_leaf() const {
  return zero == nullptr && one == nullptr;
}

huffman_decoder_tree::huffman_decoder_tree()
    : root(nullptr), current(nullptr) {}

huffman_decoder_tree::huffman_decoder_tree(
    buffered_reader& reader, std::size_t& useful_info,
    std::array<unsigned char, CHAR_BIT>& front_chars, std::size_t &read)
    : huffman_decoder_tree() {
  read = 0;
  if (reader.eof())
    return;
  unsigned char current_char = reader.next();
  std::size_t bit_read = 0;
  std::array<bool, UCHAR_MAX + 1> used = {};
  root = decode(reader, current_char, bit_read, used);
  current = root;
  useful_info = 0;
  std::size_t two_pow = 1;
  if (root->is_leaf()) {
    std::size_t left_to_read = sizeof(std::size_t) * CHAR_BIT;
    while (bit_read != CHAR_BIT || !reader.eof()) {
      bool bit = next_bit(reader, current_char, bit_read);
      if (left_to_read == 0 && bit) {
        throw std::invalid_argument("Damaged file: cannot decode header.");
      } else if (left_to_read != 0) {
        --left_to_read;
      }
      if (bit) {
        useful_info += two_pow;
      }
      two_pow *= 2;
    }
  } else {
    std::size_t max_bits = 0;
    while (two_pow < CHAR_BIT) {
      two_pow *= 2;
      ++max_bits;
    }
    two_pow = 1;
    for (std::size_t i = 0; i < max_bits; ++i) {
      if (next_bit(reader, current_char, bit_read)) {
        useful_info += two_pow;
      }
      two_pow *= 2;
    }
    if (useful_info >= CHAR_BIT) {
      throw std::invalid_argument("Damaged file: invalid number of last bits");
    }
    // reading front chars
    if (bit_read != CHAR_BIT) {
      if (useful_info != 0 && reader.eof()) {
        if (useful_info <= bit_read) {
          throw std::invalid_argument(
              "Damaged file: invalid number of last bits");
        }
        current_char <<= bit_read;

        read = get_front_chars(current_char, useful_info - bit_read, front_chars);
        if (current != root) {
          throw std::invalid_argument("Damaged file: invalid data");
        }
      } else {
        current_char <<= bit_read;
        read = get_front_chars(current_char, CHAR_BIT - bit_read, front_chars);
      }
    }
  }
}

std::size_t huffman_decoder_tree::get_front_chars(
    unsigned char c, std::size_t bit_count,
    std::array<unsigned char, CHAR_BIT>& front_chars) {
  std::size_t read = 0;
  for (std::size_t i = 0; i < bit_count; ++i) {
    if ((c & (1 << (CHAR_BIT - 1 - i))) == 0) {
      current = current->zero;
      if (current->is_leaf()) {
        front_chars[read++] = current->c;
        current = root;
      }
    } else {
      current = current->one;
      if (current->is_leaf()) {
        front_chars[read++] = current->c;
        current = root;
      }
    }
  }
  return read;
}

huffman_decoder_tree::node*
huffman_decoder_tree::decode(buffered_reader& reader,
                             unsigned char& current_char, std::size_t& bit_read,
                             std::array<bool, UCHAR_MAX + 1>& used) {
  if (next_bit(reader, current_char, bit_read)) {
    unsigned char c = next_char(reader, current_char, bit_read);
    if (used[c]) {
      throw std::invalid_argument("Damaged file: cannot decode huffman tree.");
    } else {
      used[c] = true;
    }
    return new node(c);
  } else {
    node* zero = decode(reader, current_char, bit_read, used);
    node* one = decode(reader, current_char, bit_read, used);
    return new node(zero, one);
  }
}

huffman_decoder_tree::~huffman_decoder_tree() {
  if (root != nullptr) {
    destruct(root);
  }
}

bool huffman_decoder_tree::empty() const {
  return root == nullptr;
}

bool huffman_decoder_tree::is_leaf(unsigned char& c) const {
  if (root != nullptr) {
    c = root->c;
    return root->is_leaf();
  }
  return false;
}

void huffman_decoder_tree::write(buffered_writer& writer, unsigned char c) {
  if (current->buffered_way[c] == nullptr) {
    std::size_t size = 0;
    node* temp = current;
    for (std::size_t j = 0; j < CHAR_BIT; ++j) {
      if ((c & (1 << (CHAR_BIT - 1 - j))) == 0) {
        temp = temp->zero;
        if (temp->is_leaf()) {
          current->buffered_chars[c][size++] = temp->c;
          writer.write(temp->c);
          temp = root;
        }
      } else {
        temp = temp->one;
        if (temp->is_leaf()) {
          current->buffered_chars[c][size++] = temp->c;
          writer.write(temp->c);
          temp = root;
        }
      }
    }
    current->buffered_way[c] = temp;
    current->buffered_chars[c][CHAR_BIT] = size;
    current = temp;
  } else {
    for (std::size_t i = 0; i < current->buffered_chars[c][CHAR_BIT]; ++i) {
      writer.write(current->buffered_chars[c][i]);
    }
    current = current->buffered_way[c];
  }
}

void huffman_decoder_tree::write(buffered_writer& writer, unsigned char c,
                                 std::size_t last_bits) {
  for (std::size_t i = 0; i < last_bits; ++i) {
    if ((c & (1 << (CHAR_BIT - 1 - i))) == 0) {
      current = current->zero;
      if (current->is_leaf()) {
        writer.write(current->c);
        current = root;
      }
    } else {
      current = current->one;
      if (current->is_leaf()) {
        writer.write(current->c);
        current = root;
      }
    }
  }
  if (current != root) {
    throw std::invalid_argument("Damaged file: invalid data.");
  }
}

void huffman_decoder_tree::destruct(node* cur) const {
  if (cur->zero != nullptr) {
    destruct(cur->zero);
  }
  if (cur->one != nullptr) {
    destruct(cur->one);
  }
  delete (cur);
}

bool huffman_decoder_tree::next_bit(buffered_reader& reader,
                                    unsigned char& current_char,
                                    size_t& bit_read) const {
  if (bit_read == CHAR_BIT) {
    if (reader.eof()) {
      throw std::invalid_argument("Damaged file: cannot decode file header.");
    }
    current_char = reader.next();
    bit_read = 0;
  }
  ++bit_read;
  return ((current_char & (1 << (CHAR_BIT - bit_read))) != 0);
}

unsigned char huffman_decoder_tree::next_char(buffered_reader& reader,
                                              unsigned char& current_char,
                                              size_t& bit_read) const {
  unsigned char ans = current_char << bit_read;
  if (reader.eof()) {
    throw std::invalid_argument("Damaged file: cannot decode file header.");
  }
  current_char = reader.next();
  ans |= (current_char >> (CHAR_BIT - bit_read));
  return ans;
}
