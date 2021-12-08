using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    public class Day7 : Puzzle
    {
        public Day7()
            : base(Name: "--- Day 7: The Treachery of Whales ---") { }

        public override void Part1(bool TestMode)
        {
            Data.Crabs.LoadData(TestMode);
            var positions = Data.Crabs.Positions.Clone();
            var start = DateTime.Now;
            var fuelCost = CalcFuelV1(positions);

            var timeLapse = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay7 Part1:\tFuel Cost { fuelCost }\t\t{timeLapse:F8} Seconds");
        }
        public override void Part2(bool TestMode)
        {
            Data.Crabs.LoadData(TestMode);
            var positions = Data.Crabs.Positions.Clone();
            var start = DateTime.Now;
            var fuelCost = CalcFuelV2(positions);

            var timeLapse = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay7 Part2:\tFuel Cost { fuelCost }\t\t{timeLapse:F8} Seconds");
        }
        private static int CalcFuelV1(List<int> positions)
        {
            var min = positions.Min();
            var max = positions.Max();
            var costs = new int[max - min];
            Parallel.For(min, max, pos =>
            {
                costs[pos] = positions.Sum(p => Math.Abs(p - pos));
            });

            return costs.Min();
        }
        private static int CalcFuelV2(List<int> positions)
        {
            var min = positions.Min();
            var max = positions.Max();
            var costs = new int[max - min];
            Parallel.For(min, max, pos =>
            {
                costs[pos] = positions.Sum(p => Cost(p, pos));
            });

            return costs.Min();
        }
        private static int Cost(int start, int end)
        {
            var cost = 0;
            var min = Math.Min(start, end);
            var max = Math.Max(start, end);
            var moves = 0;
            for (int i = min; i < max; i++)
                cost += ++moves;

            return cost;
        }
    }
}
