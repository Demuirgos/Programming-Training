using System;
using System.Linq;
using System.Collections.Generic;


// parse this "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green" into a dictionary
List<List<Subset>> games = new();
string[] input = @"[input here]".Split('\n');

Console.WriteLine(Day2());

Config ParseInput(string line) {
	int ParsePrefix(string line) {
		int index = 5;
		ParseWhitespace(line, ref index);
		_ = ParseValue(line, ref index);
		return index + 1;
	}
    int ParseValue(string line, ref int index) {
        int startingIndex = index;
        while(index < line.Length && Char.IsDigit(line[index])) index++;
        return Int32.Parse(line[startingIndex..index]);
    }
    void ParseWhitespace(string line, ref int index) {
        while(index < line.Length && (line[index]==' ' || line[index]==',')) index++;
    }
    Color ParseColor(string line, ref int index) {
        int startingIndex = index;
        while(index < line.Length && Char.IsLetter(line[index])) index++;
        return Enum.Parse<Color>(line[startingIndex..index]);
    }
    Subset ParseSubset(string line, ref int index) {
        Dictionary<Color, int> config = new();
        while(index < line.Length && line[index] != ';') {
            ParseWhitespace(line, ref index);
            int value = ParseValue(line, ref index);
            ParseWhitespace(line, ref index);
            Color color = ParseColor(line, ref index);
            ParseWhitespace(line, ref index);
            config[color] = value;
        }
        index++;
        return new Subset(config);
    }
	
    int index = ParsePrefix(line);
    List<Subset> game = new();

    while(index < line.Length) {
        Subset subset = ParseSubset(line, ref index);
        game.Add(subset);
    }
    return new Config(game);
}

int Day1() {
	Dictionary<Color, int> initConfig = new() {
		[Color.red] = 12,
		[Color.blue] = 14,
		[Color.green] = 13,
	};
	
	List<Config> games = input.Select(ParseInput).ToList();
	var result = 	games.Select((game, index) => (game, index))
						 .Where(item => item.game.subsets.Where(set => set.Values.Any(kvp => kvp.Value > initConfig[kvp.Key])).Any())
						 .Select(final => final.index + 1);
	int total = Enumerable.Range(1, games.Count).Sum() - result.Sum();
	return total;
}

int Day2() {
	List<Config> games = input.Select(ParseInput).ToList();
	var result = 	
        games.Select(config => {
            int maxBlue = config.subsets.Select(set => set.Values.GetValueOrDefault(Color.blue, 1)).Max();
            int maxGreen = config.subsets.Select(set => set.Values.GetValueOrDefault(Color.green, 1)).Max();
            int maxRed = config.subsets.Select(set => set.Values.GetValueOrDefault(Color.red, 1)).Max();
			return maxBlue * maxGreen * maxRed;
        });
	int total = result.Sum();
	return total;
}

public enum Color { green, red, blue }
public record Subset(Dictionary<Color, int> Values) {
	public override string ToString() => String.Join('\n', Values.Select(v => $"\t{v.Key} : {v.Value}"));
}
public record Config(List<Subset> subsets)  {
	public override string ToString() => String.Join('\n', subsets.Select((s, i) => $"hand {i} : \n{s.ToString()}"));
}
 
