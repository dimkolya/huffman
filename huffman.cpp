//
// Created by dimkolya on 02.09.22.
//

#include "huffman.h"
#include "buffered_reader.h"
#include "buffered_writer.h"
#include "huffman_code.h"
#include "huffman_decoder_tree.h"
#include "huffman_encoder_tree.h"

#include <array>
#include <climits>

inline void write_header(buffered_writer& writer,
                         std::array<std::size_t, UCHAR_MAX + 1>& frequency,
                         std::array<huffman_code, UCHAR_MAX + 1>& code_table) {}

void huffman::compress(std::istream& in, std::ostream& out) {
  buffered_reader reader(in);
  std::array<std::size_t, UCHAR_MAX + 1> frequency = {};
  while (!reader.eof()) {
    ++frequency[reader.next()];
  }
  huffman_encoder_tree tree(frequency);
  buffered_writer writer(out);
  unsigned char only_char;
  if (tree.empty()) {
    // empty file -> empty file
    return;
  } else if (tree.is_leaf(only_char)) {
    std::size_t file_size = frequency[only_char];
    tree.encode_tree(writer, file_size);
  } else {
    std::array<huffman_code, UCHAR_MAX + 1> code_table =
        tree.get_huffman_code_table();
    std::size_t last_bits = 0;
    for (std::size_t i = 0; i < UCHAR_MAX + 1; ++i) {
      if (frequency[i] != 0) {
        last_bits +=
            ((frequency[i] % CHAR_BIT) * code_table[i].get_shift()) % CHAR_BIT;
        last_bits %= CHAR_BIT;
      }
    }
    tree.encode_tree(writer, last_bits);
    reader.reset();
    while (!reader.eof()) {
      writer.write(code_table[reader.next()]);
    }
  }
}

void huffman::decompress(std::istream& in, std::ostream& out) {
  buffered_reader reader(in);
  std::size_t useful_info;
  std::array<unsigned char, CHAR_BIT> front_chars;
  std::size_t read;
  huffman_decoder_tree tree(reader, useful_info, front_chars, read);
  buffered_writer writer(out);
  unsigned char only_char;
  if (tree.empty()) {
    // empty tree (aka empty file) -> empty file
    return;
  } else if (tree.is_leaf(only_char)) {
    // useful_info are number of only_char
    for (std::size_t i = 0; i < useful_info; ++i) {
      writer.write(only_char);
    }
  } else {
    for (std::size_t i = 0; i < read; ++i) {
      writer.write(front_chars[i]);
    }
    if (useful_info != 0) {
      while (true) {
        unsigned char c = reader.next();
        if (!reader.eof()) {
          tree.write(writer, c);
        } else {
          tree.write(writer, c, useful_info);
          break;
        }
      }
    } else {
      while (!reader.eof()) {
        tree.write(writer, reader.next());
      }
    }
  }
}
