//
// Created by dimkolya on 07.05.23.
//

#ifndef HUFFMAN_HUFFMAN_ENCODER_TREE_H
#define HUFFMAN_HUFFMAN_ENCODER_TREE_H

#include "buffered_reader.h"
#include "buffered_writer.h"
#include "huffman_code.h"

#include <array>
#include <climits>

struct huffman_encoder_tree {
  huffman_encoder_tree();
  explicit huffman_encoder_tree(
      const std::array<std::size_t, UCHAR_MAX + 1>& frequency);
  ~huffman_encoder_tree();
  std::array<huffman_code, UCHAR_MAX + 1> get_huffman_code_table() const;
  void encode_tree(buffered_writer& writer, std::size_t useful_info) const;
  bool empty() const;
  bool is_leaf(unsigned char& c) const;

private:
  struct node {
    unsigned char c;
    std::size_t frequency;
    node *zero;
    node *one;

    node();
    explicit node(unsigned char c, std::size_t frequency);
    explicit node(node* zero, node* one);
    ~node();
    bool is_leaf() const;
  };

  node* root;

  void destruct(node* current) const;
  void build_code_table(node* current, huffman_code& code,
                        std::array<huffman_code, UCHAR_MAX + 1>& table) const;
  std::size_t encode(buffered_writer& writer, node* current) const;
};


#endif // HUFFMAN_HUFFMAN_ENCODER_TREE_H
