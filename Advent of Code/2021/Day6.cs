using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

long f(int n, int d) => (d - n - 0 > 0 ? 1 + f(6 , d - n  - 1) + f(8 , d - n  - 1) : 0) ;

var Solve = (int d) => File.ReadAllLines(@".\Input\Day6.txt")
  .SelectMany(l => l.Split(',').Select(Int32.Parse).ToList())
  .GroupBy(i => i)
	.Select(g => g.Count() * (1 + f(g.Key, d)))
	.Sum();
var (part1, part2) = (Solve(80), Solve(256));
