#include "pch.h"

#include "huffman.h"
#include "buffered_reader.h"
#include "buffered_writer.h"
#include "huffman_code.h"
#include "huffman_decoder_tree.h"
#include "huffman_encoder_tree.h"

#include <fstream>

// encodes input file into output file
// returns 0 on success, -1 if can't open input file,
// -2 if can't open output file, -3 invalid format of encoding file
// -4 error while encoding
int huffman_encode(
	const char *input_filename,
	const char *output_filename,
	volatile unsigned long long &analysis,
	volatile unsigned long long &encoding) {
	std::ifstream in(input_filename, std::ios::binary);
	if (!in.is_open()) {
		return -1;
	}
	std::ofstream out(output_filename, std::ios::binary);
	if (!out.is_open()) {
		return -2;
	}
	
	try {
		analysis = 0;
		encoding = 0;
		int this_analysis = 0;
		int this_encoding = 0;
		buffered_reader reader(in);
		std::array<std::size_t, UCHAR_MAX + 1> frequency = {};
		while (!reader.eof()) {
			++frequency[reader.next()];
			++this_analysis;
			if (this_analysis == 1024) {
				this_analysis = 0;
				analysis += 1024;
			}
		}
		analysis += this_analysis;
		huffman_encoder_tree tree(frequency);
		buffered_writer writer(out);
		unsigned char only_char;
		if (tree.empty()) {
			// empty file -> empty file
			analysis = 1;
			encoding = 1;
			return 0;
		}
		else if (tree.is_leaf(only_char)) {
			std::size_t file_size = frequency[only_char];
			tree.encode_tree(writer, file_size);
			encoding = file_size;
		}
		else {
			std::array<huffman_code, UCHAR_MAX + 1> code_table = tree.get_huffman_code_table();
			std::size_t last_bits = 0;
			for (std::size_t i = 0; i < UCHAR_MAX + 1; ++i) {
				last_bits += frequency[i] * code_table[i].get_shift();
			}
			last_bits %= CHAR_BIT;
			tree.encode_tree(writer, last_bits);
			reader.reset();
			while (!reader.eof()) {
				writer.write(code_table[reader.next()]);
				++this_encoding;
				if (this_encoding == 1024) {
					this_encoding = 0;
					encoding += 1024;
				}
			}
			encoding += this_encoding;
		}
	}
	catch (...) {
		return -3;
	}

	return 0;
}


// encodes input file into output file
// returns 0 on success, -1 if can't open input file,
// -2 if can't open output file, -3 invalid format of decoding file
// -4 error while decoding
int huffman_decode(
	const char* input_filename,
	const char* output_filename,
	volatile unsigned long long &decoding) {
	std::ifstream in(input_filename, std::ios::binary);
	if (!in.is_open()) {
		return -1;
	}
	std::ofstream out(output_filename, std::ios::binary);
	if (!out.is_open()) {
		return -2;
	}

	try {
		buffered_reader reader(in);
		std::size_t useful_info;
		std::array<unsigned char, CHAR_BIT> front_chars;
		std::size_t read;
		huffman_decoder_tree tree(reader, useful_info, front_chars, read);
		buffered_writer writer(out);
		unsigned char only_char;
		if (tree.empty()) {
			// empty tree (aka empty file) -> empty file
			return 0;
		}
		else if (tree.is_leaf(only_char)) {
			// useful_info are number of only_char
			for (std::size_t i = 0; i < useful_info; ++i) {
				writer.write(only_char);
			}
		}
		else {
			for (std::size_t i = 0; i < read; ++i) {
				writer.write(front_chars[i]);
			}
			if (useful_info != 0) {
				while (true) {
					unsigned char c = reader.next();
					if (!reader.eof()) {
						tree.write(writer, c);
					}
					else {
						tree.write(writer, c, useful_info);
						break;
					}
				}
			}
			else {
				while (!reader.eof()) {
					tree.write(writer, reader.next());
				}
			}
		}
	}
	catch (const std::invalid_argument& e) {
		return -3;
	}
	catch (...) {
		return -4;
	}

	return 0;
}
