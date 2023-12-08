#define DAY2
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

var digits = new Dictionary<string, int>() {
	["1"] = 1, ["2"] = 2, ["3"] = 3, ["4"] = 4, ["5"] = 5, ["6"] = 6, ["7"] = 7, ["8"] = 8, ["9"] = 9, 
};
var  number = new Dictionary<string, int>() {
	["one"] = 1, ["two"] = 2, ["three"] = 3, ["four"] = 4, ["five"] = 5, ["six"] = 6, ["seven"] = 7, ["eight"] = 8, ["nine"] = 9
};

string[] input = @"""[input here]""".Split('\n');

#if DAY1
var result = input.Select(l => GetValue(l, digits));
#else 
var result = input.Select(l => GetValue(l, number.Concat(digits).ToDictionary()));
#endif
Console.WriteLine(result.Sum());

int GetValue(String line, IDictionary<string, int> digits) {
    IEnumerable<(int, int, int)> mapp = digits
        .Select(d => (d.Value, line.IndexOf(d.Key), line.LastIndexOf(d.Key)));
    int firstDigit = mapp.Where(d => d.Item2 >= 0).OrderBy(d => d.Item2).First().Item1;
    int lastDigit  = mapp.Where(d => d.Item3 >= 0).OrderBy(d => d.Item3).Last().Item1;
    return firstDigit * 10 + lastDigit;
}

