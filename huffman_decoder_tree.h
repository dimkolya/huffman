//
// Created by dimkolya on 13.09.22.
//

#ifndef UNTITLED1_HUFFMAN_TREE_H
#define UNTITLED1_HUFFMAN_TREE_H

#include "buffered_reader.h"
#include "buffered_writer.h"

#include <array>
#include <climits>

struct huffman_decoder_tree {
  friend struct cst_huffman_tree;

  huffman_decoder_tree();
  huffman_decoder_tree(buffered_reader& reader, std::size_t& useful_info,
                       std::array<unsigned char, CHAR_BIT>& front_chars,
                       std::size_t &read);
  ~huffman_decoder_tree();
  bool empty() const;
  bool is_leaf(unsigned char& c) const;
  void write(buffered_writer& writer, unsigned char c);
  void write(buffered_writer& writer, unsigned char c, std::size_t last_bits);

private:
  struct node {
    unsigned char c;
    node* zero;
    node* one;
    // CHAR_BIT + 1 for information about size on last cell
    std::array<std::array<unsigned char, CHAR_BIT + 1>, UCHAR_MAX + 1>
        buffered_chars;
    std::array<node*, UCHAR_MAX + 1> buffered_way;

    explicit node(unsigned char c);
    node(node* zero, node* one);
    ~node();
    bool is_leaf() const;
  };

  node* root;
  node* current;

  void destruct(node* cur) const;
  node* decode(buffered_reader& reader, unsigned char& current_char,
               std::size_t& bit_read, std::array<bool, UCHAR_MAX + 1>& used);
  std::size_t get_front_chars(unsigned char c, std::size_t bit_count,
                       std::array<unsigned char, CHAR_BIT>& front_chars);
  bool next_bit(buffered_reader& reader, unsigned char& current_char,
                std::size_t& bit_read) const;
  unsigned char next_char(buffered_reader& reader, unsigned char& current_char,
                          std::size_t& bit_read) const;
};

#endif // UNTITLED1_HUFFMAN_TREE_H
