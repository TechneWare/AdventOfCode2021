using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    public interface IPuzzle
    {
        string Name { get; }
        double DayNumber { get; }
        public void Run();
        public void Part1(bool TestMode);
        public void Part2(bool TestMode);
    }
}
