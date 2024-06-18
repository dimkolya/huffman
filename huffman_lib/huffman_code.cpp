#include "pch.h"

#include "huffman_code.h"

huffman_code::huffman_code() : full_chars(0), shift(0), data() {}

huffman_code::huffman_code(const huffman_code& other)
    : full_chars(other.full_chars), shift(other.shift), data() {
    for (std::size_t i = 0; i < full_chars + (shift == 0 ? 0 : 1); ++i) {
        data[i] = other.data[i];
    }
}

huffman_code& huffman_code::operator=(const huffman_code& other) {
    if (this == &other) {
        return *this;
    }
    full_chars = other.full_chars;
    shift = other.shift;
    for (std::size_t i = 0; i < full_chars + (shift == 0 ? 0 : 1); ++i) {
        data[i] = other.data[i];
    }
    return *this;
}

huffman_code::~huffman_code() = default;

void huffman_code::push_back(bool bit) {
    data[full_chars] |= ((bit ? 1 : 0) << (CHAR_BIT - 1 - shift));
    if (shift == CHAR_BIT - 1) {
        shift = 0;
        ++full_chars;
    }
    else {
        ++shift;
    }
}

void huffman_code::pop_back() {
    if (shift == 0) {
        data[--full_chars] &= ~1;
        shift = CHAR_BIT - 1;
    }
    else {
        data[full_chars] &= (~(1 << (CHAR_BIT - shift)));
        --shift;
    }
}

std::size_t huffman_code::get_shift() const {
    return shift;
}
