using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Day1_part1(TestMode: true);
            Day1_part1(TestMode: false);
            Day1_part2(TestMode: true);
            Day1_part2(TestMode: false);
            Console.WriteLine();

            Day2_part1(TestMode: true);
            Day2_part1(TestMode: false);
            Day2_part2(TestMode: true);
            Day2_part2(TestMode: false);
            Console.WriteLine();

            Day3_part1(TestMode: true);
            Day3_part1(TestMode: false);
            Day3_part2(TestMode: true);
            Day3_part2(TestMode: false);
            Console.WriteLine();

            Console.ReadLine();
        }

        public static void Day1_part1(bool TestMode)
        {
            Data.SonarInput.TestMode = TestMode;
            var increments = 0;
            var lastValue = Data.SonarInput.Data.First();
            for (int i = 0; i < Data.SonarInput.Data.Count(); i++)
            {
                if (Data.SonarInput.Data[i] > lastValue)
                    increments++;

                lastValue = Data.SonarInput.Data[i];
            }

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay1 Part1:\tIncrements = {increments}");
        }
        public static void Day1_part2(bool TestMode)
        {
            Data.SonarInput.TestMode = TestMode;
            var increments = 0;
            var data = Data.SonarInput.DataWindowed;

            var lastValue = data.First();
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i] > lastValue)
                    increments++;

                lastValue = data[i];
            }

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay1 Part2:\tIncrements={increments}");
        }

        public static void Day2_part1(bool TestMode)
        {
            Data.SubPath.TestMode = TestMode;
            var hor = 0;
            var depth = 0;

            foreach (var d in Data.SubPath.PathData)
            {
                switch (d.Direction)
                {
                    case "forward":
                        hor += d.Value;
                        break;
                    case "down":
                        depth += d.Value;
                        break;
                    case "up":
                        depth -= d.Value;
                        break;
                    default:
                        throw new Exception("Unknown input");
                        break;
                }

                if (depth < 0) depth = 0;
                if (hor < 0) hor = 0;
            }

            var answer = hor * depth;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay2 Part1:\tAnswer = {answer}");
        }
        public static void Day2_part2(bool TestMode)
        {
            Data.SubPath.TestMode = TestMode;
            var hor = 0;
            var depth = 0;
            var aim = 0;

            foreach (var d in Data.SubPath.PathData)
            {
                switch (d.Direction)
                {
                    case "forward":
                        hor += d.Value;
                        depth += (d.Value * aim);
                        break;
                    case "down":
                        //depth += d.Value;
                        aim += d.Value;
                        break;
                    case "up":
                        //depth -= d.Value;
                        aim -= d.Value;
                        break;
                    default:
                        throw new Exception("Unknown input");
                        break;
                }

                //if (depth < 0) depth = 0;
                //if (hor < 0) hor = 0;
            }

            var answer = hor * depth;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay2 Part2:\tAnser = {answer}");
        }

        public static void Day3_part1(bool TestMode)
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
        public static void Day3_part2(bool TestMode)
        {
            Data.SubPower.TestMode= TestMode;
            int oxyValue = GetOxyValue(Data.SubPower.PowerData.Clone());
            int co2Value = GetCo2Value(Data.SubPower.PowerData.Clone());

            var lifeRating = oxyValue * co2Value;

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay3 Part2:\tLife Support Rating = {lifeRating}");
        }

        private static int GetOxyValue(List<Data.DataPoint> dataPoints)
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
                            if (dataPoints.Count() > 1) dataPoints.Remove(dp);
                    }
                    else
                    {
                        var discard = dataPoints.Where(d => d.Bits[i] == 1).ToList();
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
        private static int GetCo2Value(List<Data.DataPoint> dataPoints)
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