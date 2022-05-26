#pragma once

#define SHOW(T,V) do { T x = V; print_bits(#T, #V, (unsigned char*) &x, sizeof(x)); } while(0);
void tool_bytes2hex(unsigned char* src, char* out, int len);
void print_byte_as_bits(char val);
void print_bits(const char* ty, const char* val, unsigned char* bytes, size_t num_bytes);
