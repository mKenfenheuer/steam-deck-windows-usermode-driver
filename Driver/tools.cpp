#include <stdio.h>

char HexLookUp[] = "0123456789ABCDEF";

void tool_bytes2hex(unsigned char* src, char* out, int len)
{
    while (len--)
    {
        *out++ = HexLookUp[*src >> 4];
        *out++ = HexLookUp[*src & 0x0F];
        src++;
    }
    *out = 0;
}

void print_byte_as_bits(char val) {
    for (int i = 7; 0 <= i; i--) {
        printf("%c", (val & (1 << i)) ? '1' : '0');
    }
}

void print_bits(const char* ty, const char* val, unsigned char* bytes, size_t num_bytes) {
    printf("(%*s) %*s = [ ", 15, ty, 16, val);
    for (size_t i = 0; i < num_bytes; i++) {
        print_byte_as_bits(bytes[i]);
        printf(" ");
    }
    printf("]\n");
}