#include<iostream>
#include<fstream>
#include<vector>
#include<ranges>
#include<string_view>
#include<utility>
#include<algorithm>
#include<map>
#include<numeric>
#include <functional>

auto correctOffset(char c) 
    -> int
    {
        return c >= 'a' && c <= 'z' ? c - 'a' : c - 'A' + 26;
    }

auto resetOffset(int offset) 
    -> int
    {
        return offset >= 26 ? offset - 26 + 'A' : offset + 'a';
    }


auto readLine(std::string_view line) 
    -> std::vector<std::string_view>
    {
        std::vector<std::string_view> result;
        auto len = line.size();
        auto first = line.substr(0, len / 2);
        result.push_back(first);
        auto second = line.substr(len / 2, len / 2);
        result.push_back(second);
        return result;
    }

auto readInput(std::vector<std::string>& lines, int part)
    -> std::vector<std::vector<std::string_view>> {
        if(part == 1) {
            auto result = std::vector<std::vector<std::string_view>>{};
            std::transform( lines.cbegin(), lines.cend(),
                            std::back_inserter(result), readLine);
            return result;
        } else {
            auto result = std::vector<std::vector<std::string_view>>{};
            for (size_t i = 0; i < lines.size(); i+=3)
            {
                auto group = std::vector<std::string_view>{};
                for(int j = i; j < i + 3; ++j) {
                    group.push_back(lines[j]);
                }
                result.push_back(group);
            }
            return result;
        }
    }

auto extractDups(std::vector<std::string_view>& strings)
    -> std::vector<char>
    {
        auto duplicates = std::vector<char>{};
        int chars[26 * 2] = {0};
        for (size_t i = 0; i < strings.size(); i++)
        {
            bool memo[26 * 2] = {false};
            for (auto& c : strings[i])
            {
                auto offset = correctOffset(c);
                if(memo[offset] == false) {
                    chars[offset]++;
                    memo[offset] = true;
                }
            }
        }
        
        for (size_t i = 0; i < 26 * 2 ; i++)
        {
            if(chars[i] == strings.size()) {
                duplicates.push_back(resetOffset(i));
            }
        }
        return duplicates;
    }

auto extractValue (std::vector<char>&& dups) 
    -> int
    {
        return std::accumulate(dups.cbegin(), dups.cend(), 
            0, 
            [](int acc, char c) {
                return acc + correctOffset(c) + 1;
            }
        );
    }

auto extractResult (std::vector<std::vector<std::string_view>> container) 
    -> int 
    {
        return std::accumulate(container.cbegin(), container.cend(), 
            0, 
            [](int acc, std::vector<std::string_view> line_pair) {
                return acc + extractValue(extractDups(line_pair));
            }
        );
    }

int main() {
    // open file
    std::ifstream infile("./day3.txt");

    // read file
    std::vector<std::string> lines;
    std::string line;
    while (infile >> line) {
        lines.push_back(line);
    }

    int part1 =  extractResult(readInput(lines, 1));
    int part2 =  extractResult(readInput(lines, 2));
    std::cout << part1 << " " << part2 << std::endl;

    // close file
    infile.close();
}