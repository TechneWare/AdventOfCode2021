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
        public string Part1Result { get; set; } = "Not Run";
        public string Part2Result { get; set; } = "Not Run";

        public Puzzle(string Name, double DayNumber)
        {
            name = Name;
            dayNumber = DayNumber;
        }
        public void Run()
        {
            Console.WriteLine($"\n--- Day {DayNumber} {name} ---");

            foreach (var mode in new bool[] { true, false })
            {
                var start = DateTime.Now;
                Part1(mode);
                var duration = (DateTime.Now - start).TotalSeconds;
                Console.WriteLine($"{(mode ? "Test" : "Actual").PadRight(7)}Day {DayNumber.ToString().PadRight(4)} Part1: {Part1Result.PadRight(26)}{duration:F8} Seconds");

                start = DateTime.Now;
                Part2(mode);
                duration = (DateTime.Now - start).TotalSeconds;
                Console.WriteLine($"{(mode ? "Test" : "Actual").PadRight(7)}Day {dayNumber.ToString().PadRight(4)} Part2: {Part2Result.PadRight(26)}{duration:F8} Seconds");
            }
        }

        public abstract void Part1(bool TestMode);
        public abstract void Part2(bool TestMode);
    }
}
