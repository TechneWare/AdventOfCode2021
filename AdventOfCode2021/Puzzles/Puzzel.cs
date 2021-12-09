using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    public abstract class Puzzle : IPuzzle
    {
        private readonly string name;
        private readonly double dayNumber;
        public double DayNumber => dayNumber;
        public string Name => name;
        public Puzzle(string Name, double DayNumber)
        {
            name = Name;
            dayNumber = DayNumber;
        }
        public void Run()
        {
            Console.WriteLine($"\n{name}");

            foreach (var mode in new bool[] { true, false })
            {
                Part1(mode);
                Part2(mode);
            }
        }

        public abstract void Part1(bool TestMode);
        public abstract void Part2(bool TestMode);
    }
}
