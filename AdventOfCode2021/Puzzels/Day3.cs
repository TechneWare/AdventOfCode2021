using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzels
{
    /// <summary>
    /// --- Day 3: Binary Diagnostic ---
    /// <see cref="https://adventofcode.com/2021/day/3"/>
    /// </summary>
    public class Day3 : Puzzel
    {
        public Day3(string Name = "--- Day 3: Binary Diagnostic ---")
            : base(Name) { }

        public override void Part1(bool TestMode)
        {
            Data.SubPower.TestMode = TestMode;
            string gama = "";
            string epsilon = "";

            var width = Data.SubPower.PowerData[0].Bits.Length;
            for (int i = 0; i < width; i++)
            {
                var col = Data.SubPower.PowerData.Select(d => d.Bits[i]).ToList();
                var ones = col.Count(d => d == 1);
                var zeros = col.Count(d => d == 0);

                var gBit = ones > zeros ? 1 : 0;
                var eBit = gBit == 1 ? 0 : 1;

                gama += gBit;
                epsilon += eBit;
            }

            var g = Convert.ToInt32(gama, 2);
            var e = Convert.ToInt32(epsilon, 2);

            var power = g * e;
            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay3 Part1:\tPower={power}");
        }
        public override void Part2(bool TestMode)
        {
            Data.SubPower.TestMode = TestMode;
            int oxyValue = GetOxyValue(Data.SubPower.PowerData.Clone());
            int co2Value = GetCo2Value(Data.SubPower.PowerData.Clone());

            var lifeRating = oxyValue * co2Value;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay3 Part2:\tLife Support Rating = {lifeRating}");
        }
        private int GetOxyValue(List<Data.DataPoint> dataPoints)
        {
            int result = 0;

            if (dataPoints != null && dataPoints.Any())
            {
                var width = dataPoints.First().Bits.Count();
                for (int i = 0; i < width; i++)
                {
                    var col = dataPoints.Select(d => d.Bits[i]).ToList();
                    var ones = col.Count(d => d == 1);
                    var zeros = col.Count(d => d == 0);

                    if (ones >= zeros)
                    {
                        var discard = dataPoints.Where(d => d.Bits[i] == 0).ToList();
                        foreach (var dp in discard)
                            if (dataPoints.Count > 1) dataPoints.Remove(dp);
                    }
                    else
                    {
                        var discard = dataPoints.Where(d => d.Bits[i] == 1).ToList();
                        foreach (var dp in discard)
                            if (dataPoints.Count > 1)
                                dataPoints.Remove(dp);
                    }

                    if (dataPoints.Count == 1)
                        break;
                }

                result = dataPoints.First().ToInt();
            }

            return result;
        }
        private int GetCo2Value(List<Data.DataPoint> dataPoints)
        {
            int result = 0;

            if (dataPoints != null && dataPoints.Any())
            {
                var width = dataPoints.First().Bits.Count();
                for (int i = 0; i < width; i++)
                {
                    var col = dataPoints.Select(d => d.Bits[i]).ToList();
                    var ones = col.Count(d => d == 1);
                    var zeros = col.Count(d => d == 0);

                    if (zeros <= ones)
                    {
                        var discard = dataPoints.Where(d => d.Bits[i] == 1).ToList();
                        foreach (var dp in discard)
                            if (dataPoints.Count() > 1) dataPoints.Remove(dp);
                    }
                    else
                    {
                        var discard = dataPoints.Where(d => d.Bits[i] == 0).ToList();
                        foreach (var dp in discard)
                            if (dataPoints.Count() > 1)
                                dataPoints.Remove(dp);
                    }

                    if (dataPoints.Count == 1)
                        break;
                }

                result = dataPoints.First().ToInt();
            }

            return result;
        }
    }
}
