#include "stdio.h"
#include "string.h"
#include "stdlib.h"
#include "math.h"
#define max(a,b) (((a) > (b)) ? (a) : (b))
#define min(a,b) (((a) < (b)) ? (a) : (b))
#define abs(n)   ((n) > 0 ? (n) : -(n))
int FIN = 1;
int width = 0, height = 0;
typedef struct {
    int x1, y1;
    int x2, y2;
} line;

int* initGrid(int w, int h){
    int* grid = malloc(h * w * sizeof(int));
    memset(grid, 0, sizeof(int) * h * w);
    return grid;
}

void escapeArrow(FILE** f, int n){
    for(;n>=0; n-- ) fgetc(*f);
}

int consumeNumber(FILE** f){
    int c; int acc = 0;
    while((c = fgetc(*f)) != ',' && c != ' ' && c != '\n' && c != EOF)
        acc = acc*10 + (c - '0');
    if(c == EOF) FIN= 0 ;
    return acc;
}

line parseLine(FILE** file) {
    line r;
    r.x1 = consumeNumber(file);
    r.y1 = consumeNumber(file);
    escapeArrow(file, 2);
    r.x2 = consumeNumber(file);
    r.y2 = consumeNumber(file);
    width  = max(width , max(r.x1 + 1, r.x2 + 1));
    height = max(height, max(r.y1 + 1, r.y2 + 1));
    return r;
}

void ProcessLine(int** Grid, line* line, int part){
    int i;
    if (line->x1 == line->x2) {
        int lo = min(line->y1, line->y2), hi = max(line->y1, line->y2);
        for(i = lo; i <= hi; i++){
            (*Grid)[i  * width + line->x1] += 1;
        }
    } else if (line->y1 == line->y2) {
        int lo = min(line->x1, line->x2), hi = max(line->x1, line->x2);
        for(i = lo ; i <= hi; i++){
            (*Grid)[line->y1 * width + i] += 1;
        }
    } else if(part == 2 && abs(line->x1 -line->x2) == abs(line->y1 -line->y2)) {
        float a = atan2((line->y2 - line->y1), (line->x2 - line->x1));
        int x = line->x1, y = line->y1, k = 0;
        while(x != line->x2 && y != line->y2){
            x = line->x1 + k * (cos(a) > 0 ? 1 : -1);
            y = line->y1 + k * (sin(a) > 0 ? 1 : -1);
            (*Grid)[y * width + x] += 1; k++;
        }
    }
}

int Solve(int** Grid){
    int i ,j, acc = 0, count = 0;
    for(i = 0; i < height; i++){
        for(j = 0; j < width; j++){
            acc = max(acc, (*Grid)[i * width + j]); 
            count += (*Grid)[i * width + j] > 1;
        }
    }
    return count;
}

void main(){
    FILE* file = fopen("Input/Day5.txt", "r");
    int part = 2; // 1 => part1, 2 => part2 
    int lenLines = 100;
    line* lines = (line*)malloc(sizeof(line) * lenLines);
    if(file){
        int i = 0;
        while(FIN){
            i++;
            if(i == lenLines){
                lenLines += 100;
                lines = (line*)realloc(lines, sizeof(line) * lenLines);
            }
            lines[i - 1] = parseLine(&file);
        }
        int* Grid = initGrid(width, height);
        for(--i ;i >= 0; i--) {
            ProcessLine(&Grid, lines + i, part);
        }
        printf("%d", Solve(&Grid));
    }
}
