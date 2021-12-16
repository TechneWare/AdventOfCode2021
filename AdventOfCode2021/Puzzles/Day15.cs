using AdventOfCode2021.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 15: Chiton ---
    /// <see cref="https://adventofcode.com/2021/day/15"/>
    /// </summary>
    public class Day15 : Puzzle
    {
        public Day15()
            : base(Name: "Chiton", DayNumber: 15) { }

        private static List<ChitenPath> _chitenPathList = new();
        public override void Part1(bool TestMode)
        {
            Data.Chitin.LoadData(TestMode);
            var data = Data.Chitin.Grid;
            int? MinRisk = GetMinRisk(data);

            if (TestMode || WithLogging) data.Print(_chitenPathList);

            Part1Result = $"Risk = {MinRisk}";
        }

        public override void Part2(bool TestMode)
        {
            Data.Chitin.LoadDataTimes5(TestMode);
            var data = Data.Chitin.GridX5;
            int? MinRisk = GetMinRisk(data);

            if (TestMode || WithLogging) data.Print(_chitenPathList);

            Part2Result = $"Risk = {MinRisk}";
        }
        private int? GetMinRisk(Chitin.Point[][] data)
        {
            _chitenPathList.Clear();

            var heap = new Heap<ChitenPath>();
            var visited = data.MakeMap();
            var initialPath = new ChitenPath() { Row = 0, Col = 0, Risk = data[0][0].Risk };
            heap.Add(initialPath);
            int? MinRisk = null;
            while (true)
            {
                ChitenPath? p = heap.PopMin();
                if (p == null)
                {
                    Log("Heap Exception", "Heap was empty after Pop");
                    break;
                }

                _chitenPathList.Add(p);

                if (visited[p.Row][p.Col] > 0) continue;
                if (p.Row == data.Length - 1 && p.Col == data[0].Length - 1)
                {
                    MinRisk = p.Risk - data[0][0].Risk;
                    break;
                }

                visited[p.Row][p.Col]++;
                var newPoints = new List<(int r, int c)>()
                {
                    new (p.Row - 1, p.Col),
                    new (p.Row, p.Col + 1),
                    new (p.Row + 1, p.Col),
                    new (p.Row, p.Col - 1),
                };
                foreach (var np in newPoints)
                {
                    if (np.r < 0 || np.r > data.Length - 1 || np.c < 0 || np.c > data[data.Length - 1].Length - 1)
                        continue;
                    if (visited[np.r][np.c] > 0)
                        continue;
                    var newPath = new ChitenPath() { Row = np.r, Col = np.c, Risk = p.Risk + data[np.r][np.c].Risk };
                    heap.Add(newPath);
                }
            }

            return MinRisk;
        }
    }

    public class ChitenPath : IComparable
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Risk { get; set; }
        public ChitenPath Next { get; }

        public ChitenPath(ChitenPath next = null)
        {
            Next = next;
        }
        public int CompareTo(object? obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Object must have a value");

            var p = (ChitenPath)obj;
            if (p.Risk < this.Risk)
                return 1;
            else if (p.Risk > this.Risk)
                return -1;
            else
                return 0;
        }
    }
    public static class ChitinExtensions
    {
        public static int[][] MakeMap(this Data.Chitin.Point[][] points)
        {
            int[][] newMap = new int[points.Length][];
            for (int r = 0; r < points.Length; r++)
                newMap[r] = new int[points[r].Length];

            return newMap;
        }
        public static bool IsAdjecentTo(this ChitenPath p1, ChitenPath p2)
        {
            var sameRow = p1.Row == p2.Row;
            var sameCol = p1.Col == p2.Col;
            var adjRow = Math.Abs(p1.Row - p2.Row) == 1;
            var adjCol = Math.Abs(p1.Col - p2.Col) == 1;
            return sameRow && adjCol || sameCol && adjRow;
        }
        public static void Print(this Chitin.Point[][] map, List<ChitenPath> path)
        {
            var forColor = Console.ForegroundColor;
            var bakColor = Console.BackgroundColor;

            var pth = new List<ChitenPath>();
            for (int i = path.Count - 1; i >= 0; i--)
            {
                if (pth.Any())
                {
                    if(path[i].IsAdjecentTo(pth.First()))
                        pth.Insert(0, path[i]);
                }
                else
                    pth.Add(path[i]);
            }

            for (int r = 0; r < map.Length; r++)
            {
                for (int c = 0; c < map[r].Length; c++)
                {
                    if (pth.Any(p => p.Row == r && p.Col == c))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = ConsoleColor.Green;
                    }

                    Console.Write(map[r][c].Risk);
                    Console.ForegroundColor = forColor;
                    Console.BackgroundColor = bakColor;
                }
                Console.WriteLine();
            }
        }
    }
}
