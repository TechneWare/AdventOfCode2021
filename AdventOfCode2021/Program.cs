using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        static IEnumerable<Puzzles.IPuzzle> puzzles = new Puzzles.IPuzzle[]
        {
            new Puzzles.Day1(),
            new Puzzles.Day2(),
            new Puzzles.Day3(),
            new Puzzles.Day4(),
            new Puzzles.Day5_Trig(),
            new Puzzles.Day5_NoTrig(),
            new Puzzles.Day6(),
            new Puzzles.Day7(),
            new Puzzles.Day8(),
        };
        
        public static void Main(string[] args)
        {
            foreach (var puzzle in puzzles)
                puzzle.Run();

            Console.ReadLine();
        }
    }
}