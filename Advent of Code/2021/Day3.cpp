/******************************************************************************

                              Online C++ Compiler.
               Code, Compile, Run and Debug C++ program online.
Write your code in this editor and press "Run" button to compile and execute it.

*******************************************************************************/

#include <iostream>
#include <string>
#include <bitset>
#include <vector>
#include <algorithm>
#include <iterator>
using namespace std;
#define l 12

vector<int> iota(int s, int e){
    vector<int> r;
    for(int i=s; i<e; i++){
        r.push_back(i);
    }
    return r;
}

int part1(vector<bitset<l>>& data){
    float freqs[] = {0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,0.};
    for(int j = 0; j < l; j++){
        for(int i=0; i < data.size(); i++){
            freqs[j] += (float)data[i][j]/data.size();
        }
    }
    bitset<l> result;
    for(int j = 0; j < l; j++){
        result[j] = freqs[j]>=0.5;
    }
    return result.to_ulong() * (~result).to_ulong();
}

int part2Proc(vector<bitset<l>>& data, bool check){
    auto idxs = iota(0, data.size());
    for(int j = l - 1 ; j >= 0 && idxs.size() > 1 ; j--){
        int freq = count_if(idxs.begin(), idxs.end(), [=](int i){
            return data[i][j];
        });
        vector<int>::iterator it;
        if(freq > idxs.size() - freq){
            it = remove_if(idxs.begin(), idxs.end(), [&](int i){
                return check ? !data[i][j] : data[i][j];
            });
        } else if (freq == idxs.size() - freq){
            if(check){
                it = remove_if(idxs.begin(), idxs.end(), [=](int i){
                    return !data[i][j];
                });
            } else {
                it = remove_if(idxs.begin(), idxs.end(), [=](int i){
                    return data[i][j];
                });
            }
        } else {
            it = remove_if(idxs.begin(), idxs.end(), [=](int i){
                return check ? data[i][j] : !data[i][j];
            });
        }
        idxs.erase(it, idxs.end());
    }
    return data[idxs[0]].to_ulong();
}

int part2(vector<bitset<l>>& data){
    return part2Proc(data, true) * part2Proc(data, false);
}

int main()
{
    string line;
    vector<bitset<l>> data;
    while (cin >> line) data.push_back(std::bitset< l >( stoi(line, 0, 2) ));
    cout << "Part 1 : " << part1(data) << endl;
    cout << "Part 2 : " << part2(data) << endl;
}
