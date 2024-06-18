#pragma once

#include <array>
#include <climits>
#include <cstddef>
#include <string>

struct huffman_code {
	friend struct buffered_writer;
	huffman_code();
	huffman_code(const huffman_code& other);
	huffman_code& operator=(const huffman_code& other);
	~huffman_code();
	void push_back(bool bit);
	void pop_back();
	std::size_t get_shift() const;

private:
	std::size_t full_chars;
	std::size_t shift;
	std::array<unsigned char, (UCHAR_MAX + CHAR_BIT - 1) / CHAR_BIT> data;
};
