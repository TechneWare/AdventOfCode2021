using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzels
{
    /// <summary>
    /// --- Day 4: Giant Squid ---
    /// <see cref="https://adventofcode.com/2021/day/4"/>
    /// </summary>
    public class Day4 : Puzzel
    {
        public Day4()
            : base(Name: "--- Day 4: Giant Squid ---") { }

        public override void Part1(bool TestMode)
        {
            Data.SquidBingo.LoadData(TestMode);
            var draws = Data.SquidBingo.Draws;
            var boards = Data.SquidBingo.Boards;
            Data.Board winner = null;

            foreach (var r in draws)
            {
                foreach (var b in boards)
                {
                    b.MarkSquars(r);
                    if (b.IsWinner)
                    {
                        winner = b;
                        break;
                    }
                }

                if (winner != null)
                    break;
            }

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay4 Part1:\tFirst Winning Score = {(winner != null ? winner.Score : "No Winner")}");

        }

        public override void Part2(bool TestMode)
        {
            Data.SquidBingo.LoadData(TestMode);
            var draws = Data.SquidBingo.Draws;
            var boards = Data.SquidBingo.Boards;
            Data.Board winner = null;

            foreach (var r in draws)
                foreach (var b in boards)
                    if (!b.IsWinner)
                    {
                        b.MarkSquars(r);
                        if (b.IsWinner)
                            winner = b;
                    }

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay4 Part2:\tLast Winning Score = {(winner != null ? winner.Score : "No Winner")}");
        }
    }
}
