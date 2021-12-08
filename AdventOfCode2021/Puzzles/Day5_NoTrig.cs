using AdventOfCode2021.MathTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 5: Hydrothermal Venture ---
    /// <see cref="https://adventofcode.com/2021/day/5"/>
    /// </summary>
    public class Day5_NoTrig : Puzzle
    {
        public Day5_NoTrig()
            : base(Name: "--- Day 5: Hydrothermal Venture (No Trig) ---") { }

        public override void Part1(bool TestMode)
        {
            Data.HydroThermal.LoadData(TestMode);

            var start = DateTime.Now;

            var pointsInSystem =
                Data.HydroThermal.Lines.Where(l => l.IsHorizontal || l.IsVertical)
                .SelectMany(l => l.PointsNoTrig(step: 1)
                .Select(p => new { p.X, p.Y }))
                .ToArray();

            int OverlapCount = 0;
            var overlapLock = new object();
            _ = Parallel.ForEach(pointsInSystem.Distinct(), (point) =>
            {
                if (pointsInSystem.Where(p => p.X == point.X)
                                  .Count(p => p.Y == point.Y) > 1)
                    lock (overlapLock)
                        OverlapCount++;
            });

            var timeLapse = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay5 Part1:\tOverlaps { OverlapCount }\t\t{timeLapse:F4} Seconds");
        }

        public override void Part2(bool TestMode)
        {
            Data.HydroThermal.LoadData(TestMode);

            var start = DateTime.Now;

            var pointsInSystem =
                Data.HydroThermal.Lines
                .SelectMany(l => l.PointsNoTrig(step: 1)
                .Select(p => new { p.X, p.Y }))
                .ToArray();

            int OverlapCount = 0;
            var overlapLock = new object();
            _ = Parallel.ForEach(pointsInSystem.Distinct(), (point) =>
            {
                if (pointsInSystem.Where(p => p.X == point.X)
                                  .Count(p => p.Y == point.Y) > 1)
                    lock (overlapLock)
                        OverlapCount++;
            });

            var timeLapse = (DateTime.Now - start).TotalSeconds;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay5 Part2:\tOverlaps { OverlapCount }\t\t{timeLapse:F4} Seconds");
        }
    }
}
