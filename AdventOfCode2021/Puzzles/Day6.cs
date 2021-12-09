using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 6: Lanternfish ---
    /// <see cref="https://adventofcode.com/2021/day/6"/>
    /// </summary>
    public class Day6 : Puzzle
    {
        public Day6()
            : base(Name: "--- Day 6: Lanternfish ---", DayNumber: 6) { }

        public override void Part1(bool TestMode)
        {
            Data.LaternFish.LoadData(TestMode);
            var fishData = Data.LaternFish.Fish.Clone().ToArray();
            long[] fish = new long[9];
            for (int day = 0; day < fish.Length; day++)
                fish[day] = fishData.Count(d => d == day);
            
            var start = DateTime.Now;
            var maxDays = 80;
            for (int day = 0; day < maxDays; day++)
                fish.Update();

            var timeLapse = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay6 Part1:\tTotal Fish { fish.Sum() }\t\t{timeLapse:F8} Seconds");
        }

        public override void Part2(bool TestMode)
        {
            Data.LaternFish.LoadData(TestMode);

            var fishData = Data.LaternFish.Fish.Clone().ToArray();
            long[] fish = new long[9];
            for (int day = 0; day < fish.Length; day++)
                fish[day] = fishData.Count(d => d == day);

            var start = DateTime.Now;
            var maxDays = 256;
            for (int day = 0; day < maxDays; day++)
                fish.Update();

            var timeLapse = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay6 Part2:\tTotal Fish { fish.Sum() }\t\t{timeLapse:F8} Seconds");
        }
    }

    public static class FishExtensions
    {
        public static void Update(this long[] days)
        {
            long d0 = days[0];
            for (int d = 1; d < days.Length; d++)
                days[d - 1] = days[d];
            days[8] = d0;
            days[6] += d0;
        }
    }
}
