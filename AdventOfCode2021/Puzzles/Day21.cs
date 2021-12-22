using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Numpy;
using Numpy.Models;
using Python.Runtime;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 21: Dirac Dice ---
    /// <see cref="https://adventofcode.com/2021/day/21"/>
    /// </summary>
    public class Day21 : Puzzle
    {
        public Day21()
            : base(Name: "Dirac Dice", DayNumber: 21) { }

        public override void Part1(bool TestMode)
        {
            Game.Reset();
            if (TestMode)
            {
                Game.Players.Add(new Player(PlayerNumber: 1, startPosition: 4));
                Game.Players.Add(new Player(PlayerNumber: 2, startPosition: 8));
            }
            else
            {
                Game.Players.Add(new Player(PlayerNumber: 1, startPosition: 4));
                Game.Players.Add(new Player(PlayerNumber: 2, startPosition: 5));
            }

            while (!Game.HasWinner)
                Game.PlayRound();

            var loserScore = Game.Loser.Score;
            var diceRolls = Dice.NumRoles;

            Part1Result = $"Answer = {loserScore * diceRolls}";
        }

        public override void Part2(bool TestMode)
        {
            List<int> pos;
            if (TestMode)
                pos = new List<int>() { 4, 8 };
            else
                pos = new List<int>() { 4, 5 };

            long[] wins = QuantumGames(pos);
            long max = wins.Max();

            Part2Result = $"MaxWins = {max}";
        }

        public static class Dice
        {
            public static int NumRoles { get; internal set; }
            private static int NextValue = 0;
            public static void Reset()
            {
                NumRoles = 0;
                NextValue = 0;
            }
            public static int Roll()
            {
                NextValue++;
                NumRoles++;
                if (NextValue > 100)
                    NextValue = 1;

                return NextValue;
            }
        }
        public class Player
        {
            public int Score { get; set; }
            public int PlayerNumber { get; }
            public int Position { get; set; }
            public bool IsWinner => this.Score >= 21;
            public Player(int PlayerNumber, int startPosition)
            {
                this.PlayerNumber = PlayerNumber;
                Position = startPosition;
            }
        }
        public static class Game
        {
            public static List<Player> Players { get; set; } = new List<Player>();
            public static Player Winner { get; internal set; }
            public static Player Loser { get; internal set; }

            public static bool HasWinner => Players.Any(p => p.Score >= 1000);
            
            public static void Reset()
            {
                Dice.Reset();
                Players.Clear();
                Winner = null;
                Loser = null;
            }

            public static void PlayRound()
            {
                foreach (Player player in Players)
                {
                    var rolls = new List<int>();
                    for (int i = 0; i < 3; i++)
                        rolls.Add(Dice.Roll());

                    foreach (var roll in rolls)
                    {
                        for (int m = 0; m < roll; m++)
                        {
                            player.Position++;
                            if (player.Position > 10)
                                player.Position = 1;
                        }
                    }

                    player.Score += player.Position;

                    if (HasWinner)
                        break;
                }

                if (HasWinner)
                {
                    Winner = Players.Where(p => p.Score >= 1000).SingleOrDefault();
                    Loser = Players.Where(p => p.PlayerNumber != Winner.PlayerNumber).SingleOrDefault();
                }
            }
        }
        public static long[] QuantumGames(List<int> startPositions)
        {
            int a = startPositions[0];
            int b = startPositions[1];

            var dp = np.zeros((22, 22, 11, 11, 2), dtype: np.int64);
            dp[0, 0, a, b, 0] = (NDarray)1;
            var coef = new int[] { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };

            for (int pa = 0; pa < 21; pa++)
            {
                for (int pb = 0; pb < 21; pb++)
                {
                    for (int q = 0; q < 2; q++)
                    {
                        for (int _a = 1; _a < 11; _a++)
                        {
                            for (int _b = 1; _b < 11; _b++)
                            {
                                if ((long)dp[pa, pb, _a, _b, q] == 0)
                                    continue;

                                if (q == 0)
                                {
                                    for (int die = 3; die < 10; die++)
                                    {
                                        var a1 = (_a + die - 1) % 10 + 1;
                                        var pa1 = Math.Min(21, pa + a1);
                                        dp[pa1, pb, a1, _b, 1 - q] += coef[die] * (long)dp[pa, pb, _a, _b, q];
                                    }
                                }
                                else
                                {
                                    for (int die = 3; die < 10; die++)
                                    {
                                        var b1 = (_b + die - 1) % 10 + 1;
                                        var pb1 = Math.Min(21, pb + b1);
                                        dp[pa, pb1, _a, b1, 1 - q] += coef[die] * (long)dp[pa, pb, _a, _b, q];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var awin = (long)np.sum(dp["21, :, :,: , :"]);
            var bwin = (long)np.sum(dp[":, 21, :, :, :"]);

            return new long[] { awin, bwin };
        }
    }
}
