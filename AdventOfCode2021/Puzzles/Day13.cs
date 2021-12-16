using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 13: Transparent Origami ---
    /// <see cref="https://adventofcode.com/2021/day/13"/>
    /// </summary>
    public class Day13 : Puzzle
    {
        public Day13()
            : base(Name: "Transparent Origami", DayNumber: 13) { }

        public override void Part1(bool TestMode)
        {
            Data.Oragami.LoadData(TestMode);
            Data.Oragami.Page?.Fold();

            var CountOfMarkers = Data.Oragami.Page?.Points.SelectMany(p => p).Where(IsSet => IsSet == true).Count();
            Part1Result = $"Dots = {CountOfMarkers}";
        }

        public override void Part2(bool TestMode)
        {
            bool canFold;
            do { canFold = Data.Oragami.Page.Fold(); }
            while (canFold);

            Data.Oragami.Page.Print();

            Part2Result = "";
        }
    }
}
