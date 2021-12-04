using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzels
{
    public class Puzzel : IPuzzel
    {
        public string Name { get; set; }
        public void Run()
        {
            Console.WriteLine($"\n{Name}");

            foreach (var mode in new bool[] { true, false })
            {
                Part1(mode);
                Part2(mode);
            }
        }

        public virtual void Part1(bool TestMode) { }
        public virtual void Part2(bool TestMode) { }
    }
}
