using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzels
{
    public abstract class Puzzel : IPuzzel
    {
        private readonly string name;

        public Puzzel(string Name)
        {
            name = Name;
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
