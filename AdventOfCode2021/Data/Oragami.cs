using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class Oragami
    {
        public static Paper? Page { get; set; }

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/OragamiInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/OragamiTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            var data = dataRaw.Split("\r\n").ToList();

            var markers = new List<Point>();
            var folds = new List<Fold>();
            foreach (var line in data)
            {
                var d = line.Split(',');
                if (d.Length == 2)
                    markers.Add(new Point(int.Parse(d[0]), int.Parse(d[1])));
                else
                {
                    d = line.Split("fold along ");
                    if (d.Length == 2)
                    {
                        var fold = d[1].Split('=');
                        if (fold[0] == "x")
                            folds.Add(new Fold(Fold.FoldDirections.Left, int.Parse(fold[1])));
                        if (fold[0] == "y")
                            folds.Add(new Fold(Fold.FoldDirections.Up, int.Parse(fold[1])));
                    }
                }
            }

            Page = new Paper(markers, folds);
        }
        public class Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        public class Fold
        {
            public enum FoldDirections
            {
                Up,
                Left
            }
            public FoldDirections FoldDir { get; set; }
            public int Value { get; set; }
            public Fold(FoldDirections foldDirection, int value)
            {
                FoldDir = foldDirection;
                Value = value;
            }
        }
        public class Paper
        {
            public bool[][] Points { get; internal set; }
            public List<Fold> Folds { get; internal set; }

            public Paper(List<Point> Markers, List<Fold> foldInstructions)
            {
                var maxX = Markers.Max(m => m.X) + 1;
                var maxY = Markers.Max(m => m.Y) + 1;

                //make the paper
                Points = new bool[maxX][];
                for (int r = 0; r < maxX; r++)
                    Points[r] = new bool[maxY];

                //set the markers
                for (int x = 0; x < maxX; x++)
                    for (int y = 0; y < maxY; y++)
                        if (Markers.Any(m => m.X == x && m.Y == y))
                            Points[x][y] = true;

                Folds = foldInstructions;
            }
            public void Print()
            {
                Console.WriteLine();
                var defaultForColor = Console.ForegroundColor;
                var defaultBackColor = Console.BackgroundColor;

                for (int y = 0; y < Points[0].Length; y++)
                {
                    for (int x = 0; x < Points.Length; x++)
                        if (Points[x][y])
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write("#");
                            Console.ForegroundColor = defaultForColor;
                            Console.BackgroundColor = defaultBackColor;
                        }
                        else
                            Console.Write(".");

                    Console.WriteLine();
                }
            }

            public bool Fold()
            {
                var result = Folds.Any();

                if (result)
                {
                    var thisFold = Folds.First();
                    Folds.RemoveAt(0);

                    if (thisFold.FoldDir == Data.Oragami.Fold.FoldDirections.Left)
                    {
                        var left = new bool[thisFold.Value][];
                        var right = new bool[Points.Length - thisFold.Value + 1][];
                        int x1 = Points.Length - 1;
                        for (int x = 0; x < thisFold.Value; x++)
                        {
                            left[x] = new bool[Points[x].Length];
                            right[x] = new bool[Points[x].Length];
                            for (int y = 0; y < Points[x].Length; y++)
                            {
                                left[x][y] = Points[x][y];
                                right[x][y] = Points[x1][y];
                            }
                            x1--;
                        }

                        for (int x = 0; x < left.Length; x++)
                            for (int y = 0; y < left[0].Length; y++)
                                left[x][y] |= right[x][y];

                        Points = left;
                    }
                    else
                    {
                        var top = new bool[Points.Length][];
                        var bottom = new bool[top.Length][];
                        for (int x = 0; x < top.Length; x++)
                        {
                            int y1 = Points[x].Length - 1;
                            top[x] = new bool[thisFold.Value];
                            bottom[x] = new bool[Points[x].Length - thisFold.Value + 1];
                            for (int y = 0; y < thisFold.Value; y++)
                            {
                                top[x][y] = Points[x][y];
                                bottom[x][y] = Points[x][y1--];
                            }
                        }

                        for (int x = 0; x < top.Length; x++)
                            for (int y = 0; y < top[0].Length; y++)
                                top[x][y] |= bottom[x][y];

                        Points = top;
                    }
                }

                return result;
            }
        }
    }
}
