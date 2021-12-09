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
            : base(Name: "The Treachery of Whales", DayNumber: 7) { }

        public override void Part1(bool TestMode)
        {
            Data.Crabs.LoadData(TestMode);
            var positions = Data.Crabs.Positions;
            var fuelCost = CalcFuelV1(positions);

            Part1Result = $"Fuel Cost { fuelCost }";
        }
        public override void Part2(bool TestMode)
        {
            Data.Crabs.LoadData(TestMode);
            var positions = Data.Crabs.Positions;
            var start = DateTime.Now;
            var fuelCost = CalcFuelV2(positions);

            var timeLapse = (DateTime.Now - start).TotalSeconds;
            Part2Result = $"Fuel Cost { fuelCost }";
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
