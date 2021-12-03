using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Day1_part1();
            Day1_part2();
            Console.WriteLine();

            Day2_part1();
            Day2_part2();
            Console.WriteLine();

            Day3_part1();
            Day3_part2();
            Console.WriteLine();

            Console.ReadLine();
        }

        public static void Day1_part1()
        {
            var increments = 0;
            var lastValue = Data.SonarInput.Data.First();
            for (int i = 0; i < Data.SonarInput.Data.Count(); i++)
            {
                if (Data.SonarInput.Data[i] > lastValue)
                    increments++;

                lastValue = Data.SonarInput.Data[i];
            }

            Console.WriteLine($"Day1 Part1:\tIncrements = {increments}");
        }
        public static void Day1_part2()
        {
            var increments = 0;
            var data = Data.SonarInput.DataWindowed;

            var lastValue = data.First();
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i] > lastValue)
                    increments++;

                lastValue = data[i];
            }

            Console.WriteLine($"Day1 Part2:\tIncrements={increments}");
        }

        public static void Day2_part1()
        {
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

            Console.WriteLine($"Day2 Parts:\tAnswer = {answer}");
        }
        public static void Day2_part2()
        {
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

            Console.WriteLine($"DAy2 Part2:\tAnser = {answer}");
        }

        public static void Day3_part1()
        {
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
            Console.WriteLine($"Day3 Part1:\tPower={power}");
        }
        public static void Day3_part2()
        {
            var width = Data.SubPower.PowerData[0].Bits.Length;
            var oxy = Data.SubPower.PowerData.Clone();
            var co2 = Data.SubPower.PowerData.Clone();

            for (int i = 0; i < width; i++)
            {
                var col = oxy.Select(d => d.Bits[i]).ToList();
                var ones = col.Count(d => d == 1);
                var zeros = col.Count(d => d == 0);

                if (ones >= zeros)
                    foreach (var dp in oxy.Where(d => d.Bits[i] == 0))
                        if (oxy.Count() > 1) oxy.Remove(dp);
                else
                    foreach (var dp1 in oxy.Where(d => d.Bits[i] == 1))
                        if (oxy.Count() > 1)
                            oxy.Remove(dp1);

                if (oxy.Count == 1)
                    break;
            }
            var oxyValue = Convert.ToInt32(oxy.First()._value, 2);

            for (int i = 0; i < width; i++)
            {
                var col = co2.Select(d => d.Bits[i]).ToList();

                var ones = col.Count(d => d == 1);
                var zeros = col.Count(d => d == 0);

                if (zeros <= ones)
                {
                    var discard = co2.Where(d => d.Bits[i] == 1).ToList();
                    foreach (var d in discard)
                        if (co2.Count() > 1)
                            co2.Remove(d);
                }
                else
                {
                    var discard = co2.Where(d => d.Bits[i] == 0).ToList();
                    foreach (var d in discard)
                        if (co2.Count() > 1)
                            co2.Remove(d);
                }

                if (co2.Count() == 1)
                    break;
            }

            var co2Value = Convert.ToInt32(co2.First()._value, 2);

            var lifeRating = oxyValue * co2Value;

            Console.WriteLine($"Day3 Part2:\tLife Support Rating = {lifeRating}");

        }
    }
}