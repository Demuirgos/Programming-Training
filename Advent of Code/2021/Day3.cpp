#include <iostream>
#include <string>
#include <bitset>
#include <vector>
using namespace std;

int part1(vector<bitset<12>>& data){
    float freqs[] = {0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.};
    for(int j = 0; j < 12; j++){
        for(int i=0; i < data.size(); i++){
            freqs[j] += (float)data[i][j]/data.size();
        }
    }
    bitset<12> result;
    for(int j = 0; j < 12; j++){
        result[j] = freqs[j]>=0.5;
    }
    return result.to_ulong() * (~result).to_ulong();
}


int main()
{
    string line;
    vector<bitset<12>> data;
    while (cin >> line) data.push_back(std::bitset< 12 >( stoi(line, 0, 2) ));
    cout << "Part 1 : " << part1(data) << endl;
    //cout << "Part 2 : " << part2(data) << endl;
}
