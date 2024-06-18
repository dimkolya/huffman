#include "pch.h"

#include "huffman_encoder_tree.h"

#include <algorithm>

huffman_encoder_tree::node::node()
    : c('\0'), frequency(0), zero(nullptr), one(nullptr) {};

huffman_encoder_tree::node::node(unsigned char c, std::size_t frequency)
    : c(c), frequency(frequency), zero(nullptr), one(nullptr) {}

huffman_encoder_tree::node::node(node* zero, node* one)
    : c('\0'), frequency(zero->frequency + one->frequency), zero(zero),
    one(one) {};

huffman_encoder_tree::node::~node() = default;

bool huffman_encoder_tree::node::is_leaf() const {
    return zero == nullptr && one == nullptr;
}

huffman_encoder_tree::huffman_encoder_tree() : root(nullptr) {}

huffman_encoder_tree::huffman_encoder_tree(
    const std::array<std::size_t, UCHAR_MAX + 1>& frequency)
    : huffman_encoder_tree() {
    std::size_t count = 0;
    std::array<node*, UCHAR_MAX + 1> arr;
    for (std::size_t i = 0; i < UCHAR_MAX + 1; ++i) {
        if (frequency[i] != 0) {
            arr[count++] = new node(i, frequency[i]);
        }
    }
    if (count == 0) {
        return;
    }
    else {
        std::sort(
            std::begin(arr), std::begin(arr) + count,
            [](node* zero, node* one) { return zero->frequency > one->frequency; });
        for (std::size_t i = count - 1; i != 0; --i) {
            node* temp = new node(arr[i], arr[i - 1]);
            arr[i - 1] = temp;
            std::size_t j = i - 1;
            while (j != 0 && temp->frequency > arr[j - 1]->frequency) {
                std::swap(arr[j], arr[j - 1]);
                --j;
            }
        }
        root = arr[0];
    }
}

huffman_encoder_tree::~huffman_encoder_tree() {
    if (root != nullptr) {
        destruct(root);
    }
}

void huffman_encoder_tree::destruct(node* current) const {
    if (current->zero != nullptr) {
        destruct(current->zero);
    }
    if (current->one != nullptr) {
        destruct(current->one);
    }
    delete (current);
}

std::array<huffman_code, UCHAR_MAX + 1>
huffman_encoder_tree::get_huffman_code_table() const {
    huffman_code temp;
    std::array<huffman_code, UCHAR_MAX + 1> ans;
    if (root != nullptr) {
        build_code_table(root, temp, ans);
    }
    return ans;
}

void huffman_encoder_tree::build_code_table(
    huffman_encoder_tree::node* current, huffman_code& code,
    std::array<huffman_code, UCHAR_MAX + 1>& table) const {
    if (current->is_leaf()) {
        table[current->c] = code;
    }
    else {
        code.push_back(false);
        build_code_table(current->zero, code, table);
        code.pop_back();
        code.push_back(true);
        build_code_table(current->one, code, table);
        code.pop_back();
    }
}
void huffman_encoder_tree::encode_tree(buffered_writer& writer,
    std::size_t useful_info) const {
    if (root == nullptr) {
        // do nothing for empty tree (aka empty file)
    }
    else {
        std::size_t written = encode(writer, root);
        if (root->is_leaf()) {
            // only char and count (aka useful_info)
            while (useful_info != 0) {
                if (useful_info % 2 == 1) {
                    writer.write(true);
                }
                else {
                    writer.write(false);
                }
                useful_info /= 2;
            }
        }
        else {
            std::size_t max_bits = 0;
            std::size_t two_pow = 1;
            while (two_pow < CHAR_BIT) {
                ++max_bits;
                two_pow *= 2;
            }
            useful_info = (useful_info + max_bits + written) % CHAR_BIT;
            for (std::size_t i = 0; i < max_bits; ++i) {
                if (useful_info % 2 == 1) {
                    writer.write(true);
                }
                else {
                    writer.write(false);
                }
                useful_info /= 2;
            }
        }
    }
}

bool huffman_encoder_tree::empty() const {
    return root == nullptr;
}

bool huffman_encoder_tree::is_leaf(unsigned char& c) const {
    if (root != nullptr) {
        c = root->c;
        return root->is_leaf();
    }
    return false;
}

std::size_t huffman_encoder_tree::get_encoded_tree_size()
{
    if (root == nullptr) {
        return 0;
    }
    else {
        return encoded_size(root);
    }
}

std::size_t huffman_encoder_tree::encode(buffered_writer& writer, node* current) const {
    if (current->is_leaf()) {
        writer.write(true);
        writer.write(current->c);
        return 1 + CHAR_BIT;
    }
    else {
        writer.write(false);
        std::size_t written = 1;
        written += encode(writer, current->zero);
        written += encode(writer, current->one);
        return written;
    }
}

std::size_t huffman_encoder_tree::encoded_size(node* current)
{
    if (current->is_leaf()) {
        return 1 + CHAR_BIT;
    }
    else {
        return 1 + encoded_size(current->zero) + encoded_size(current->one);
    }
}
