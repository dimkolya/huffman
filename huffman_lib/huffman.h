#pragma once

#include <cstdint>

#ifdef HUFLIBRARY_EXPORTS
#define HUFLIBRARY_API __declspec(dllexport)
#else
#define HUFLIBRARY_API __declspec(dllimport)
#endif

// both of this functions
// returns 0 on success, -1 if can't open input file,
// -2 if can't open output file, -3 invalid format of coding file
// -4 error while coding (unknown)

extern "C" HUFLIBRARY_API int huffman_encode(
	const char* input_filename,
	const char* output_filename,
	volatile unsigned long long& analysis,
	volatile unsigned long long& encoding);
extern "C" HUFLIBRARY_API int huffman_decode(
	const char* input_filename,
	const char* output_filename,
	volatile unsigned long long &decoding);
