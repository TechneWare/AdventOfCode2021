using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 17: Trick Shot ---
    /// <see cref="https://adventofcode.com/2021/day/17"/>
    /// </summary>
    public class Day17 : Puzzle
    {
        public Day17()
            : base(Name: "Trick Shot", DayNumber: 17) { }

        public override void Part1(bool TestMode)
        {
            Data.TargetArea.LoadData(TestMode);

            RunSim(Data.TargetArea.Target, Data.TargetArea.TargetBounds,
                   out Probe? bestProbe,
                   out int yMax);

            if (WithLogging && bestProbe != null)
            {
                bestProbe.Print();
                Log("Best Path", $"Steps = {bestProbe.Path.Count}");
            }

            Part1Result = $"YMax = {yMax}";
        }


        public override void Part2(bool TestMode)
        {
            Data.TargetArea.LoadData(TestMode);

            var goodProbes = RunSim(Data.TargetArea.Target, Data.TargetArea.TargetBounds,
                                    out Probe? bestProbe,
                                    out int yMax);

            Part2Result = $"Good Vel = {goodProbes.Count}";
        }
        private static List<Probe> RunSim(List<(int x, int y)> target, (int xMin, int xMax, int yMin, int yMax) targetBounds, out Probe? bestProbe, out int yMax)
        {
            var successfulProbes = new List<Probe>();
            int xLow = Math.Min(targetBounds.xMin, 0);
            int xHigh = Math.Max(targetBounds.xMax, 0);
            int yLow = Math.Min(targetBounds.yMin, 0);
            int yHigh = Math.Max(Math.Abs(targetBounds.yMin), Math.Abs(targetBounds.yMax));
            bestProbe = null;
            yMax = 0;
            for (int velX = xLow; velX <= xHigh + 1; velX++)
            {
                for (int velY = yLow; velY <= yHigh + 1; velY++)
                {
                    var probe = new Probe(target, targetBounds);
                    probe.Fire(velX, velY);

                    while (!probe.IsHit() && !probe.IsMiss())
                        probe.Update();

                    if (probe.IsHit())
                    {
                        successfulProbes.Add(probe);
                        int newYmax = Math.Max(yMax, probe.Path.Max(p => p.y));
                        if (newYmax > yMax)
                        {
                            yMax = newYmax;
                            bestProbe = probe;
                        }
                    }
                }
            }

            return successfulProbes;
        }
    }

    public class Probe
    {
        private readonly List<(int x, int y)> targetPoints;
        private readonly (int xMin, int xMax, int yMin, int yMax) targetBounds;
        private int xVel = 0;
        private int yVel = 0;

        public int xPos { get; set; } = 0;
        public int yPos { get; set; } = 0;
        public List<(int x, int y)> Path { get; set; } = new List<(int x, int y)>();

        public Probe(List<(int x, int y)> TargetPoints,
                          (int xMin, int xMax, int yMin, int yMax) TargetBounds)
        {
            targetPoints = TargetPoints;
            targetBounds = TargetBounds;
        }

        public void Fire(int x, int y)
        {
            xVel = x;
            yVel = y;

            Path.Add(new(xPos, yPos));
        }
        public void Update()
        {
            xPos += xVel;
            yPos += yVel;
            Path.Add(new(xPos, yPos));

            if (xVel > 0)
                xVel -= 1;
            else if (xVel < 0)
                xVel += 1;

            yVel -= 1;
        }

        public bool IsMiss()
        {
            bool isMiss = (xVel > 0 && xPos > targetBounds.xMax) ||
                          (xVel <= 0 && xPos < targetBounds.xMin) ||
                          (yVel <= 0 && yPos < targetBounds.yMin);

            return isMiss;
        }
        public bool IsHit()
        {
            bool isHit = targetBounds.xMin <= xPos && xPos <= targetBounds.xMax &&
                         targetBounds.yMin <= yPos && yPos <= targetBounds.yMax;
            return isHit;
        }
        public void Print()
        {
            var forColor = Console.ForegroundColor;
            var bakColor = Console.BackgroundColor;

            var minX = Path.Min(p => p.x);
            var maxX = Math.Max(Path.Max(p => p.x), targetBounds.xMax);
            var minY = Math.Min(Path.Min(p => p.y), targetBounds.yMin);
            var maxY = Path.Max(p => p.y);

            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (Path.Any(pt => pt.x == x && pt.y == y))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("#");
                    }
                    else
                    {
                        if (targetPoints.Any(pt => pt.x == x && pt.y == y))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write("T");
                        }
                        else
                            Console.Write(".");
                    }
                    Console.ForegroundColor = forColor;
                    Console.BackgroundColor = bakColor;
                }
                Console.WriteLine();
            }
        }
    }
}
