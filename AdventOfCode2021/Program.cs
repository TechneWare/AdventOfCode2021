﻿using System;
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
            new Puzzels.Day5_NoTrig()
        };
        
        public static void Main(string[] args)
        {
            foreach (var puzzel in puzzels)
                puzzel.Run();

            Console.ReadLine();
        }
    }
}