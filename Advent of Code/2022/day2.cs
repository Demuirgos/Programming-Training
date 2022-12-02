using System.Collections.Generic;
using System.Linq;

var result = System.IO.File.ReadAllLines("./day2.txt")
#if PART1
                    .Select(s => new Input(Input.Map(s[0]), Input.Map(s[2])))
#else
                    .Select(s => new Input(Input.Map(s[0]), Input.Decide(Input.Map(s[0]), s[2])))
#endif
                    .Select(r => r.Result)
                    .Select(r => r.PScore)
                    .Sum();
Console.WriteLine(result);

public enum RockPaperScissors
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

public enum GameState
{
    Loss = 0,
    Draw = 3,
    Win = 6
}


public record Input(RockPaperScissors Opponent, RockPaperScissors Player) {
    public (int OScore, int PScore) Result {
        get {
            var gameState = Play();
            return gameState switch {
                GameState.Win => ((int)Opponent, (int)Player + (int)gameState),
                GameState.Draw => ((int)Opponent + (int)gameState, (int)Player + (int)gameState),
                GameState.Loss => ((int)Opponent + (int)gameState, (int)Player),
                _ => throw new Exception("Invalid game state")
            };
        }
    }

    public GameState Play() {
        var playerWon = Rules(this.Opponent).LosesTo == this.Player;
        return this.Player == this.Opponent ? GameState.Draw : playerWon ? GameState.Win : GameState.Loss;
    }

    public static RockPaperScissors Map(char c) => c switch {
        'A' or 'X' => RockPaperScissors.Rock,
        'B' or 'Y' => RockPaperScissors.Paper,
        'C' or 'Z' => RockPaperScissors.Scissors,
        _ => throw new System.Exception()
    };

    public static (RockPaperScissors LosesTo, RockPaperScissors WinsAgainst) Rules(RockPaperScissors hand) => hand switch {
        RockPaperScissors.Rock => (RockPaperScissors.Paper, RockPaperScissors.Scissors),
        RockPaperScissors.Paper => (RockPaperScissors.Scissors, RockPaperScissors.Rock),
        RockPaperScissors.Scissors => (RockPaperScissors.Rock, RockPaperScissors.Paper),
        _ => throw new System.Exception()
    };

    public static RockPaperScissors Decide(RockPaperScissors hand, char c) => c switch {
        'X' => Rules(hand).WinsAgainst,
        'Y' => hand,
        'Z' => Rules(hand).LosesTo,
        _ => throw new System.Exception()
    };
}