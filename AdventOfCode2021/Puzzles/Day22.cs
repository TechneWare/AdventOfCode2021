using AdventOfCode2021.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 22: Reactor Reboot ---
    /// <see cref="https://adventofcode.com/2021/day/22"/>
    /// </summary>
    public class Day22 : Puzzle
    {
        public Day22()
            : base(Name: "Reactor Reboot", DayNumber: 22) { }

        public override void Part1(bool TestMode)
        {
            Data.Reactor.LoadData(TestMode, TestPart: 1);
            long total = Data.Reactor.RebootInstructions.RunReactorReboot(maxRange: 50);

            Part1Result = $"Cubes= {total}";
        }

        public override void Part2(bool TestMode)
        {
            Data.Reactor.LoadData(TestMode, TestPart: 2);
            var total = Data.Reactor.RebootInstructions.RunReactorReboot(maxRange: int.MaxValue);

            Part2Result = $"Cubes= {total}";
        }
    }

    public static class ReactorExtensions
    {
        public static long RunReactorReboot(this List<RebootStep> steps, int maxRange)
        {

            long GetCountAfterStep(int stepIndex, Cube region)
            {
                if (region.IsEmpty || stepIndex < 0)
                    return 0;
                else
                {
                    Cube RegionIntersect = region.Intersect(steps[stepIndex].Region);
                    long OnInRegion = GetCountAfterStep(stepIndex - 1, region);
                    long OnInIntersect = GetCountAfterStep(stepIndex - 1, RegionIntersect);
                    long OnOutsideIntersect = OnInRegion - OnInIntersect;

                    return steps[stepIndex].TurnOff ? OnOutsideIntersect : OnOutsideIntersect + RegionIntersect.Volume;
                }
            }

            return GetCountAfterStep(steps.Count - 1, new Cube(new Segment(-maxRange, maxRange),
                                                               new Segment(-maxRange, maxRange),
                                                               new Segment(-maxRange, maxRange)));
        }
    }
}
