using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 2: Dive! ---
    /// <see cref="https://adventofcode.com/2021/day/2"/>
    /// </summary>
    public class Day2 : Puzzle
    {
        public Day2(string Name = "--- Day 2: Dive! ---")
            : base(Name) { }

        public override void Part1(bool TestMode)
        {
            Data.SubPath.TestMode = TestMode;
            var hor = 0;
            var depth = 0;

            foreach (var d in Data.SubPath.PathData)
            {
                switch (d.Direction)
                {
                    case "forward":
                        hor += d.Value;
                        break;
                    case "down":
                        depth += d.Value;
                        break;
                    case "up":
                        depth -= d.Value;
                        break;
                    default:
                        throw new Exception("Unknown input");
                        break;
                }

                if (depth < 0) depth = 0;
                if (hor < 0) hor = 0;
            }

            var answer = hor * depth;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay2 Part1:\tAnswer = {answer}");
        }
  
        public override void Part2(bool TestMode)
        {
            Data.SubPath.TestMode = TestMode;
            var hor = 0;
            var depth = 0;
            var aim = 0;

            foreach (var d in Data.SubPath.PathData)
            {
                switch (d.Direction)
                {
                    case "forward":
                        hor += d.Value;
                        depth += (d.Value * aim);
                        break;
                    case "down":
                        aim += d.Value;
                        break;
                    case "up":
                        aim -= d.Value;
                        break;
                    default:
                        throw new Exception("Unknown input");
                        break;
                }

                if (depth < 0) depth = 0;
                if (hor < 0) hor = 0;
            }

            var answer = hor * depth;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay2 Part2:\tAnser = {answer}");
        }
    }
}
