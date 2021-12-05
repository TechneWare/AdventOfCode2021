using AdventOfCode2021.MathTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzels
{
    /// <summary>
    /// --- Day 5: Hydrothermal Venture ---
    /// <see cref="https://adventofcode.com/2021/day/5"/>
    /// </summary>
    public class Day5_Trig : Puzzel
    {
        public Day5_Trig()
            : base(Name: "--- Day 5: Hydrothermal Venture (Using Trig) ---") { }

        public override void Part1(bool TestMode)
        {
            Data.HydroThermal.LoadData(TestMode);

            var start = DateTime.Now;
            var pointsInSystem = new List<PointF>();
            var linesToCheck = new List<Line>();
            linesToCheck.AddRange(Data.HydroThermal.Lines.Where(l => l.IsHorizontal || l.IsVertical));

            foreach (var line in linesToCheck)
                pointsInSystem.AddRange(line.Points(step: 1).ToInt());

            int OverlapCount = 0;
            var overLapLock = new object();
            var distinctPoints = pointsInSystem.Distinct().ToList();
            Parallel.ForEach(distinctPoints, (point) =>
            {
                var matchedPoints = pointsInSystem.Where(p => p.X == point.X && p.Y == point.Y).ToList();
                if (matchedPoints.Count > 1)
                    lock (overLapLock)
                        OverlapCount++;
            });

            var timeLapse = (DateTime.Now - start).TotalSeconds;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay5 Part1:\tOverlaps { OverlapCount }\t\t{timeLapse:F4} Seconds");
        }

        public override void Part2(bool TestMode)
        {
            Data.HydroThermal.LoadData(TestMode);

            var start = DateTime.Now;
            var pointsInSystem = new List<PointF>();
            var linesToCheck = Data.HydroThermal.Lines;

            foreach (var line in linesToCheck)
                pointsInSystem.AddRange(line.Points(step: 1).ToInt());

            int OverlapCount = 0;
            var overLapLock = new object();
            var distinctPoints = pointsInSystem.Distinct().ToList();

            Parallel.ForEach(distinctPoints, (point) =>
            {
                var matchedPoints = pointsInSystem.Where(p => p.X == point.X && p.Y == point.Y).ToList();
                if (matchedPoints.Count > 1)
                    lock (overLapLock)
                        OverlapCount++;
            });

            var timeLapse = (DateTime.Now - start).TotalSeconds;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay5 Part2:\tOverlaps { OverlapCount }\t\t{timeLapse:F4} Seconds");
        }
    }
}
