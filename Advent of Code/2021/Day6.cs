using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

var Solve = (int d) => File.ReadAllLines(@".\Input\Day6.txt")
  .SelectMany(l => l.Split(',').Select(Int32.Parse).ToList())
  .GroupBy(i => i)
	.Select(g => g.Count() * (1 + f(g.Key, d)))
	.Sum();
var (part1, part2) = (Solve(80), Solve(256));
