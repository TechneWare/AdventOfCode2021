using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 9: Smoke Basin ---
    /// <see cref="https://adventofcode.com/2021/day/9"/>
    /// </summary>
    public class Day9 : Puzzle
    {
        public Day9()
            : base(Name: "Smoke Basin", DayNumber: 9) { }

        public override void Part1(bool TestMode)
        {
            Data.SmokeBasin.LoadData(TestMode);
            var heatMap = Data.SmokeBasin.HeatMap;
            int[][] risk = GetRiskMap(heatMap);

            var result = risk.Sum(r => r.Sum());

            Part1Result = $"Risk Sum = {result}";
        }

        public override void Part2(bool TestMode)
        {
            Data.SmokeBasin.LoadData(TestMode);
            var heatMap = Data.SmokeBasin.HeatMap;

            List<int[][]> basins = GetBasins(heatMap);
            var totals = basins
                        .Select(b => b.Sum(b => b.Count(b => b != 0)))
                        .OrderByDescending(s => s)
                        .Take(3).ToList();

            var answer = totals[0] * totals[1] * totals[2];

            Part2Result = $"Basin Sum = {answer}";
        }

        private static List<int[][]> GetBasins(int[][] heatMap)
        {
            var result = new List<int[][]>();

            var riskMap = GetRiskMap(heatMap);
            for (int r = 0; r < riskMap.Length; r++)
            {
                for (int c = 0; c < riskMap[r].Length; c++)
                {
                    if (riskMap[r][c] > 0)
                    {
                        //found a basin, fill it out
                        int[][] basinMap = GetBasinMap(heatMap, r, c);
                        result.Add(basinMap);
                    }
                }
            }

            return result;
        }
        private static int[][] GetBasinMap(int[][] heatMap, int tRow, int tCol)
        {
            int[][] basinMap = MakeMapFrom(heatMap);

            if (heatMap[tRow][tCol] == 0)
                heatMap[tRow][tCol] = -1;

            SetBasinRow(heatMap, basinMap, tRow, tCol);
            //SetBasinCol(heatMap, basinMap, tRow, tCol);

            return basinMap;
        }
        private static void SetBasinRow(int[][] heatMap, int[][] basinMap, int tRow, int tCol)
        {
            //find left side of row
            var leftCol = tCol - 1;
            while (leftCol >= 0)
            {
                if (heatMap[tRow][leftCol] < 9)
                    leftCol--;
                else
                {
                    leftCol++;
                    break;
                }
            }
            leftCol = Math.Max(leftCol, 0);

            //find right side of row
            var rightCol = tCol + 1;
            while (rightCol <= heatMap[tRow].Length - 1)
            {
                if (heatMap[tRow][rightCol] < 9)
                    rightCol++;
                else
                {
                    //rightCol--;
                    break;
                }
            }
            rightCol = Math.Min(rightCol, heatMap[tRow].Length);

            //fill in row left to right
            for (int i = leftCol; i < rightCol; i++)
            {
                if (basinMap[tRow][i] != heatMap[tRow][i] && heatMap[tRow][i] != 9)
                {
                    basinMap[tRow][i] = heatMap[tRow][i];
                    SetBasinCol(heatMap, basinMap, tRow, i);
                }
            }
        }
        private static void SetBasinCol(int[][] heatMap, int[][] basinMap, int tRow, int tCol)
        {
            //find top of col
            var top = tRow - 1;
            while (top >= 0)
            {
                if (heatMap[top][tCol] < 9)
                    top--;
                else
                {
                    top++;
                    break;
                }
            }
            top = Math.Max(top, 0);

            //find bottom of col
            var bottom = tRow + 1;
            while (bottom <= heatMap.Length - 1)
            {
                if (heatMap[bottom][tCol] < 9)
                    bottom++;
                else
                {
                    //bottom--;
                    break;
                }
            }
            bottom = Math.Min(bottom, heatMap.Length);

            //fill in col top to bottom
            for (int i = top; i < bottom; i++)
            {
                if (basinMap[i][tCol] != heatMap[i][tCol] && heatMap[i][tCol] != 9)
                {
                    basinMap[i][tCol] = heatMap[i][tCol];
                    SetBasinRow(heatMap, basinMap, i, tCol);
                }
            }
        }
        private static int[][] GetRiskMap(int[][] heatMap)
        {
            int[][] risk = MakeMapFrom(heatMap);

            for (int r = 0; r < heatMap.Length; r++)
                for (int c = 0; c < heatMap[r].Length; c++)
                    if (!IsPeak(heatMap, r, c))
                    {
                        var minValue = GetMinValue(heatMap, r, c);
                        var isMin = heatMap[r][c] == minValue;
                        if (isMin)
                            risk[r][c] = 1 + heatMap[r][c];
                    }

            return risk;
        }
        private static int[][] MakeMapFrom(int[][] Map)
        {
            int[][] risk = new int[Map.Length][];
            for (int r = 0; r < Map.Length; r++)
                risk[r] = new int[Map[r].Length];
            return risk;
        }
        private static int GetMinValue(int[][] Map, int tRow, int tCol)
        {
            var minValue = Map[tRow][tCol];

            if (tCol > 0)
                minValue = Math.Min(minValue, Map[tRow][tCol - 1]);
            if (tCol < Map[tRow].Length - 1)
                minValue = Math.Min(minValue, Map[tRow][tCol + 1]);
            if (tRow > 0)
                minValue = Math.Min(minValue, Map[tRow - 1][tCol]);
            if (tRow < Map.Length - 1)
                minValue = Math.Min(minValue, Map[tRow + 1][tCol]);

            return minValue;
        }
        private static bool IsPeak(int[][] Map, int tRow, int tCol)
        {
            bool result = false;

            int[] targets = new int[5];
            targets[0] = Map[tRow][tCol]; //current target
            targets[1] = 9; //left
            targets[2] = 9; //right
            targets[3] = 9; //top
            targets[4] = 9; //bottom

            if (tCol > 0)
                targets[1] = Map[tRow][tCol - 1];
            if (tCol < Map[tRow].Length - 1)
                targets[2] = Map[tRow][tCol + 1];
            if (tRow > 0)
                targets[3] = Map[tRow - 1][tCol];
            if (tRow < Map.Length - 1)
                targets[4] = Map[tRow + 1][tCol];

            result = targets.All(t => t == targets[0]); //True if all values are equal

            return result;
        }
    }
}
