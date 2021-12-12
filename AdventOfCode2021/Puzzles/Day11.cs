using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 11: Dumbo Octopus ---
    /// <see cref="https://adventofcode.com/2021/day/11"/>
    /// </summary>
    public class Day11 : Puzzle
    {
        public Day11()
            : base(Name: "Dumbo Octopus", DayNumber: 11) { }

        public override void Part1(bool TestMode)
        {
            Data.Octopus.LoadData(TestMode);
            var input = Data.Octopus.Inputs.Copy();

            OctoExtensions.TotalFlashes = 0;
            for (int step = 0; step < 100; step++)
                input.Step();

            //input.PrintMap();

            Part1Result = $"Flashes = {OctoExtensions.TotalFlashes}";
        }
        public override void Part2(bool TestMode)
        {
            Data.Octopus.LoadData(TestMode);
            var input = Data.Octopus.Inputs.Copy();
            OctoExtensions.TotalFlashes = 0;

            bool isFullFlash = false;
            int[][]? flashMap = null;
            int step = 0;

            do
            {
                flashMap = input.Step();
                isFullFlash = flashMap.SelectMany(f => f).All(f => f == 1);
                step++;
            } while (!isFullFlash);

            //input.PrintMap();

            Part2Result = $"Sync Step = {step}";
        }
    }

    public static class OctoExtensions
    {
        public static int TotalFlashes { get; set; } = 0;
        public static int[][] Step(this int[][] Map)
        {
            return Map.AddEnergy().PropigateEnergy();
        }
        public static int[][] AddEnergy(this int[][] Map)
        {
            for (int r = 0; r < Map.Length; r++)
                for (int c = 0; c < Map[r].Length; c++)
                    Map[r][c]++;

            return Map;
        }
        public static int[][] PropigateEnergy(this int[][] Map)
        {
            var flashMap = Map.CloneStructure();
            for (int r = 0; r < Map.Length; r++)
                for (int c = 0; c < Map[r].Length; c++)
                    if (Map[r][c] > 9)
                        Map.Flash(r, c, flashMap);

            for (int r = 0; r < Map.Length; r++)
                for (int c = 0; c < Map[r].Length; c++)
                    if (flashMap[r][c] > 0)
                        Map[r][c] = 0;

            return flashMap;
        }
        public static void Flash(this int[][] Map, int fRow, int fCol, int[][] flashMap)
        {
            TotalFlashes++;
            flashMap[fRow][fCol]++;

            var coords = new List<int[]>
            {
                new int[] { fRow-1, fCol-1 },   //Up Left
                new int[] { fRow-1, fCol },     //Up 
                new int[] { fRow-1, fCol+1 },   //Up Right
                new int[] { fRow, fCol+1 },     //Right
                new int[] { fRow+1, fCol+1 },   //Down Right
                new int[] { fRow+1, fCol },     //Down
                new int[] { fRow+1, fCol-1 },   //Down Left
                new int[] { fRow, fCol-1 },     //Left
            };

            foreach (var coord in coords)
                if (coord.IsValidCoord(Map))
                {
                    var r = coord[0];
                    var c = coord[1];
                    var oldVal = Map[r][c];
                    Map[r][c]++;
                    if (Map[r][c] > 9 && flashMap[r][c] == 0) //If hasn't flashed, then flash
                    {
                        Map.Flash(r, c, flashMap);
                    }
                }

            if (Map[fRow][fCol] > 9)
                Map[fRow][fCol] = 0;
        }
        public static bool IsValidCoord(this int[] coord, int[][] Map)
        {
            return (coord[0] >= 0 && coord[0] < Map.Length) &&
                   (coord[1] >= 0 && coord[1] < Map[0].Length);
        }
        public static int[][] Copy(this int[][] Map)
        {
            var newMap = new int[Map.Length][];
            for (int r = 0; r < Map.Length; r++)
            {
                newMap[r] = new int[Map[r].Length];
                for (int c = 0; c < Map[r].Length; c++)
                    newMap[r][c] = Map[r][c];
            }

            return newMap;
        }
        private static int[][] CloneStructure(this int[][] Map)
        {
            int[][] newMap = new int[Map.Length][];
            for (int r = 0; r < Map.Length; r++)
                newMap[r] = new int[Map[r].Length];

            return newMap;
        }
        public static void PrintMap(this int[][] Map, int hRow = -1, int hCol = -1)
        {
            for (int r = 0; r < Map.Length; r++)
            {
                var oldColor = Console.ForegroundColor;
                for (int c = 0; c < Map[r].Length; c++)
                {
                    if (Map[r][c] > 0)
                        Console.ForegroundColor = ConsoleColor.White;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;

                    if (r == hRow && c == hCol)
                        Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.Write(Map[r][c]);
                    Console.ForegroundColor = oldColor;
                }
                Console.WriteLine();
            }
        }
    }
}
