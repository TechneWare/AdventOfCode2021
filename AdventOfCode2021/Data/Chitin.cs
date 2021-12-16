using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class Chitin
    {
        public static Point[][] Grid { get; set; } = { };
        public static Point[][] GridX5 { get; set; } = { };
        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/ChitinInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/ChitinTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            var data = dataRaw.Split("\r\n").ToList();
            Grid = new Point[data.Count][];
            int row = 0;
            foreach (var item in data)
            {
                int col = 0;
                Grid[row] = new Point[item.Length];
                foreach (var point in item)
                {
                    Grid[row][col] = new Point(row, col, int.Parse(point.ToString()));
                    col++;
                }
                row++;
            }
        }
        public static void LoadDataTimes5(bool TestMode)
        {
            LoadData(TestMode);

            GridX5 = new Point[Grid.Length * 5][];
            for (int r1 = 0; r1 < Grid[0].Length * 5; r1++)
                GridX5[r1] = new Point[Grid[0].Length * 5];

            int r = Grid.Length;
            int c = Grid[0].Length;

            for (int i = 0; i < GridX5.Length; i++)
            {
                for (int j = 0; j < GridX5[r].Length; j++)
                {
                    GridX5[i][j] = new Point(i, j, Wrap(Grid[i % r][j % c].Risk + i / r + j / c));
                }
            }

            //Print(GridX5);
            //Console.ReadLine();

            static void Print(Point[][] grid)
            {
                for (int r = 0; r < GridX5.Length; r++) 
                {
                    for (int c = 0; c < GridX5[r].Length; c++)
                        Console.Write(GridX5[r][c].Risk);
                    Console.WriteLine();
                }
            }

            static int Wrap(int x)
            {
                return (x - 1) % 9 + 1;
            }
        }
        public class Point
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public int Risk { get; set; }
            public Point(int row, int col, int risk)
            {
                this.Row = row;
                this.Col = col;
                Risk = risk;
            }
        }
    }
}
