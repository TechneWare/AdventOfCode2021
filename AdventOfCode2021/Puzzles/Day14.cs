using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 14: Extended Polymerization ---
    /// <see cref="https://adventofcode.com/2021/day/14"/>
    /// </summary>
    public class Day14 : Puzzle
    {
        public Day14()
            : base(Name: "Extended Polymerization", DayNumber: 14) { }

        public override void Part1(bool TestMode)
        {
            Data.Polymer.LoadData(TestMode);
            int steps = 10;
            var diff = GetDiff(steps);

            Part1Result = $"Diff = {diff}";
        }
        public override void Part2(bool TestMode)
        {
            Data.Polymer.LoadData(TestMode);
            int steps = 40;
            var diff = GetDiff(steps);

            Part2Result = $"Diff = {diff}";
        }
        private static long GetDiff(int steps)
        {
            long diff = 0;

            string poly = Data.Polymer.Template;
            var C1 = new Counter();

            for (int j = 0; j < poly.Length - 1; j++)
                C1.AddOrUpdate($"{poly[j]}{poly[j + 1]}", 1);

            for (int j = 0; j < steps; j++)
            {
                var C2 = new Counter();
                foreach (var i in C1.Items)
                {
                    var r = Data.Polymer.Rules.Where(rule => rule.Name == i.Key).Single();
                    C2.AddOrUpdate($"{i.Key[0]}{r.Value}", C1.Value(i.Key));
                    C2.AddOrUpdate($"{r.Value}{i.Key[1]}", C1.Value(i.Key));
                }

                C1 = C2;
            }

            var CF = new Counter();
            foreach (var i in C1.Items)
                CF.AddOrUpdate(i.Key[0].ToString(), i.Value);
            CF.AddOrUpdate(poly.Last().ToString(), 1);

            var min = CF.Items.Min(i => i.Value);
            var max = CF.Items.Max(i => i.Value);
            diff = max - min;

            return diff;
        }
        public class Counter
        {
            public List<Pair> Items { get; set; } = new List<Pair>();

            public void AddOrUpdate(string key, long count)
            {
                var p = Items.Where(p => p.Key == key).SingleOrDefault();
                if (p == null)
                {
                    p = new Pair() { Key = key, Value = 0 };
                    Items.Add(p);
                }

                p.Value += count;
            }
            public long Value(string key)
            {
                long value = 0;

                var p = Items.Where(p => p.Key == key).SingleOrDefault();
                if (p != null)
                    value = p.Value;

                return value;
            }
        }
        public class Pair
        {
            public string Key { get; set; }
            public long Value { get; set; }
        }
    }
}
