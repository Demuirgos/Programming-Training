#include<cstdio>
#define max(a,b) (a>b?a:b)

int main(int argc, char const *argv[])
{
    FILE* file = fopen("day6.txt", "r");
    int part = 2;
    if (file == NULL) {
        return 1;
    }
    int alphabet[26] = {0};
    int start = 0, index = 1;
    int count = part == 2 ? 14 : 4;
    while(!feof(file)) {
        char c = fgetc(file);
        if(alphabet[c - 'a'] != start) {
            start = max(start, alphabet[c - 'a']);
        }

        alphabet[c - 'a'] = index;
        if (index - start == count) {
            printf("%d", index);
            return 0;
        }
        index++;
    }
    
    return 0;
}
