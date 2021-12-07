using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
List<int> Yield(List<int> l, params int[] i) { l.AddRange(i); return l; }

var input = File.ReadAllLines(@".\Input\Day6.txt").SelectMany(l => l.Split(',').Select(Int32.Parse).ToList()).ToList();
var Next = (List<int> previous) => previous.Aggregate(new List<int>(), (acc, item) => item != 0 ? Yield(acc, item -1): Yield(acc, 6, 8));
List<int> Engine(List<int> state, int idx) => idx == 0 ? state : Engine(Next(state), idx - 1);

var part1 = Engine(input, 80);
var part2 = Engine(part1, 256-80);
