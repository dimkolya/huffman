cmake_minimum_required(VERSION 3.21)
project(huffman)

set(CMAKE_CXX_STANDARD 17)

find_package(GTest REQUIRED PATHS)

# NOTE: huffman functional must be implemented as library and linked to the executables
# NOTE: tool target name must be huffman-tool

add_library(huffman_lib
        huffman.cpp
        huffman_decoder_tree.cpp
        huffman_encoder_tree.cpp
        buffered_writer.cpp
        buffered_reader.cpp
        huffman_code.cpp huffman_encoder_tree.h)

add_executable(tests unit-tests/tests.cpp)

add_executable(huffman-tool huffman_tool.cpp)

target_link_libraries(tests huffman_lib)

target_link_libraries(huffman-tool PUBLIC huffman_lib)

# don't forget to use same flags at your library 
if (NOT MSVC)
    target_compile_options(tests PRIVATE -Wall -Wno-sign-compare -pedantic)
endif ()

option(USE_SANITIZERS "Enable to build with undefined,leak and address sanitizers" OFF)
if (USE_SANITIZERS)
    target_compile_options(tests PUBLIC -fsanitize=address,undefined,leak -fno-sanitize-recover=all)
    target_link_options(tests PUBLIC -fsanitize=address,undefined,leak)
endif ()

if (CMAKE_CXX_COMPILER_ID MATCHES "Clang")
    target_compile_options(tests PUBLIC -stdlib=libc++)
endif ()

if (CMAKE_BUILD_TYPE MATCHES "Debug")
    target_compile_options(tests PUBLIC -D_GLIBCXX_DEBUG)
endif ()

target_link_libraries(tests GTest::gtest GTest::gtest_main)
