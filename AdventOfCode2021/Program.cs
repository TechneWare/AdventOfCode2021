using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        static IEnumerable<Puzzels.IPuzzel> puzzels = new Puzzels.IPuzzel[]
        {
            new Puzzels.Day1(),
            new Puzzels.Day2(),
            new Puzzels.Day3(),
            new Puzzels.Day4(),
            new Puzzels.Day5_Trig(),
            new Puzzels.Day5_NoTrig(),
            new Puzzels.Day6(),
            new Puzzels.Day7(),
        };
        
        public static void Main(string[] args)
        {
            foreach (var puzzel in puzzels)
                puzzel.Run();

            Console.ReadLine();
        }
    }
}