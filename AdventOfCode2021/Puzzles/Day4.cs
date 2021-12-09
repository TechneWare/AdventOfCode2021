using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 4: Giant Squid ---
    /// <see cref="https://adventofcode.com/2021/day/4"/>
    /// </summary>
    public class Day4 : Puzzle
    {
        public Day4()
            : base(Name: "--- Day 4: Giant Squid ---", DayNumber: 4) { }

        public override void Part1(bool TestMode)
        {
            Data.SquidBingo.LoadData(TestMode);
            var draws = Data.SquidBingo.Draws;
            var boards = Data.SquidBingo.Boards;
            Data.Board? winner = null;

            foreach (var num in draws)
            {
                foreach (var board in boards)
                {
                    board.MarkSquars(num);
                    if (board.IsWinner)
                    {
                        winner = board;
                        break;
                    }
                }

                if (winner != null)
                    break;
            }

            Part1Result = $"Day4 Part1:\tFirst Win = {(winner != null ? winner.Score : "No Winner")}";
        }

        public override void Part2(bool TestMode)
        {
            Data.SquidBingo.LoadData(TestMode);
            var draws = Data.SquidBingo.Draws;
            var boards = Data.SquidBingo.Boards;
            Data.Board? winner = null;

            foreach (var num in draws)
                foreach (var board in boards)
                    if (!board.IsWinner)
                    {
                        board.MarkSquars(num);
                        if (board.IsWinner)
                            winner = board;
                    }

           Part2Result = $"Day4 Part2:\tLast Win = {(winner != null ? winner.Score : "No Winner")}";
        }
    }
}
