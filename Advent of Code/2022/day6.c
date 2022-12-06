#include<cstdio>
#define max(a,b) (a>b?a:b)

int main(int argc, char const *argv[])
{
    FILE* file = fopen("day6.txt", "r");
    int part = 2;
    if (file == NULL) {
        printf("Error opening file");
    }
    int alphabet[26] = {0};
    int start = 0, index = 1;
    int count = part == 2 ? 14 : 4;
    while(!feof(file)) {
        char c = fgetc(file);
        if(alphabet[c - 'a'] == start) {
            alphabet[c - 'a'] = index;
        } else {
            start = max(start, alphabet[c - 'a']);
            alphabet[c - 'a'] = index;
        }

        if (index - start == count) {
            printf("%d", index);
            break;
        }
        index++;
    }
    
    return 0;
}
