using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 19: Beacon Scanner ---
    /// <see cref="https://adventofcode.com/2021/day/19"/>
    /// </summary>
    public class Day19 : Puzzle
    {
        public Day19()
            : base(Name: "Beacon Scanner", DayNumber: 19) { }

        public override void Part1(bool TestMode)
        {
            Data.BeaconScanner.LoadData(TestMode);

            var scanners = Data.BeaconScanner.DataRaw.ReadScanners();
            
            List<(int x, int y, int z)> offsets;
            List<(int x, int y, int z)>? result = GetBeacons(scanners, out offsets);
            Part1Result = $"Num Beacons= {result.Count}";

            int max = offsets.GetMaxDistance();
            Part2Result = $"Max Dist = {max}";
        }


        public override void Part2(bool TestMode)
        { 
            //Completed on part1, just let result displayed
        }
        private static List<(int x, int y, int z)> GetBeacons(List<List<(int x, int y, int z)>> scanners,
                                                              out List<(int x, int y, int z)> offsets)
        {
            var result = new List<(int x, int y, int z)>();
            var offs = new List<(int x, int y, int z)>();
            var visited = new List<int>();

            void SearchBeacons(int i,
                               List<(int x, int y, int z)> beacons,
                               (int x, int y, int z) offset)
            {
                result?.AddRange(beacons.Except(result));
                offs?.Add(offset);
                visited?.Add(i);

                foreach (var scanner in scanners)
                {
                    i = scanners.IndexOf(scanner);
                    if (visited.Contains(i))
                        continue;

                    var match = beacons.GetMatches(scanner, out (int x, int y, int z) o);
                    if (match != null)
                        SearchBeacons(i, match, o);
                }
            }

            SearchBeacons(0, scanners[0], (x: 0, y: 0, z: 0));
            offsets = offs;

            return result;
        }
    }

    public static class BeaconScannerExtensions
    {
        private static (int rx, int ry, int rz)[] Rotations()
        {
            var range = Enumerable.Range(0, 4);
            var product = (from x in range
                           from y in range
                           from z in range
                           select (rx: x, ry: y, rz: z)).ToArray();

            return product;
        }
        private static (int x, int y, int z) Rotate((int x, int y, int z) point, 
                                                    (int rx, int ry, int rz) rotation)
        {
            (int x, int y, int z) p = new(point.x, point.y, point.z);

            foreach (var _ in Enumerable.Range(0, rotation.rx))
                p = (p.x, p.z, -p.y);

            foreach (var _ in Enumerable.Range(0, rotation.ry))
                p = (p.z, p.y, -p.x);

            foreach (var _ in Enumerable.Range(0, rotation.rz))
                p = (p.y, -p.x, p.z);

            return p;
        }
        private static List<(int x, int y, int z)> RotateBeacons(List<(int x, int y, int z)> Beacons, (int rx, int ry, int rz) rotation)
        {
            var rotatedBeacons = new List<(int x, int y, int z)>();
            foreach (var b in Beacons)
            {
                var rb = Rotate(b, rotation);
                rotatedBeacons.Add(rb);
            }
            return rotatedBeacons;
        }
        private static (int x, int y, int z) GetOffset(this (int x, int y, int z) p1, (int x, int y, int z) p2)
        {
            return new(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
        }
        private static List<(int x, int y, int z)> GetOffsetBeacons(this List<(int x, int y, int z)> beacons, (int x, int y, int z) offset)
        {
            var result = new List<(int x, int y, int z)>();

            foreach (var b in beacons)
                result.Add(new(b.x + offset.x, b.y + offset.y, b.z + offset.z));

            return result;
        }
        public static List<(int x, int y, int z)> GetMatches(this List<(int x, int y, int z)> originBeacons, List<(int x, int y, int z)> Beacons, out (int x, int y, int z) offset)
        {
            foreach (var rotation in Rotations())
            {
                var rotatedBeacons = RotateBeacons(Beacons, rotation);
                foreach (var originBeacon in originBeacons)
                {
                    foreach (var beacon in rotatedBeacons)
                    {
                        offset = originBeacon.GetOffset(beacon);
                        var offsetBeacons = rotatedBeacons.GetOffsetBeacons(offset);

                        var matches = offsetBeacons.Where(b => originBeacons.Contains(b)).ToList();

                        if (matches.Count >= 12)
                            return offsetBeacons;
                    }
                }
            }

            offset = new();
            return null;
        }
        public static int GetDistance(this (int x, int y, int z) p1, (int x, int y, int z) p2)
        {
            return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y) + Math.Abs(p1.z - p2.z);
        }
        public static int GetMaxDistance(this List<(int x, int y, int z)>? offsets)
        {
            int max = 0;
            for (int i = 0; i < offsets.Count; i++)
            {
                for (int j = 0; j < offsets.Count; j++)
                {
                    if (i != j)
                    {
                        var diff = offsets[i].GetDistance(offsets[j]);
                        max = Math.Max(max, diff);
                    }
                }
            }

            return max;
        }
        public static List<List<(int x, int y, int z)>> ReadScanners(this List<string> dataRaw)
        {
            var scanners = new List<List<(int x, int y, int z)>>();
            List<(int x, int y, int z)> curScanner = null;
            foreach (var line in dataRaw)
            {
                if (line.Contains("scanner"))
                {
                    curScanner = new List<(int x, int y, int z)>();
                    scanners.Add(curScanner);
                }
                else if (curScanner != null && line.Contains(','))
                {
                    var p = line.Split(',').Select(d => int.Parse(d)).ToArray();
                    (int x, int y, int z) point = new(p[0], p[1], p[2]);
                    curScanner.Add(point);
                }
            }

            return scanners;
        }
    }
}
