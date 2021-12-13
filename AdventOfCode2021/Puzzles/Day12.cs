using AdventOfCode2021.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 12: Passage Pathing ---
    /// <see cref="https://adventofcode.com/2021/day/12"/>
    /// </summary>
    public class Day12 : Puzzle
    {
        public Day12()
            : base(Name: "Passage Pathing", DayNumber: 12) { }

        public override void Part1(bool TestMode)
        {
            Data.PassagePathing.LoadData(TestMode);

            Node startNode = PassagePathing.Nodes.Single(n => n.Name == "start");
            Node endNode = PassagePathing.Nodes.Single(n => n.Name == "end");
            List<Path> AllPaths = startNode.GetAllPathsToEnd(PassagePathing.Nodes,
                                                             PassageExtensions.Version.One);
            //AllPaths.Print();

            Part1Result = $"Paths = {AllPaths.Count}";
        }

        public override void Part2(bool TestMode)
        {
            Data.PassagePathing.LoadData(TestMode);

            Node startNode = PassagePathing.Nodes.Single(n => n.Name == "start");
            Node endNode = PassagePathing.Nodes.Single(n => n.Name == "end");
            List<Path> AllPaths = startNode.GetAllPathsToEnd(PassagePathing.Nodes,
                                                             PassageExtensions.Version.Two);
            //AllPaths.Print();
            Part2Result = $"Paths = {AllPaths.Count}";
        }
    }
    public class Path
    {
        public Node StartNode => Nodes.First();
        public Node EndNode => Nodes.Last();
        public List<Node> Nodes { get; set; }
        public Path()
        {
            Nodes = new List<Node>();
        }
        public Path(Node startNode)
        {
            Nodes = new List<Node> { startNode };
        }
        public void AddNode(Node aNode)
        {
            Nodes.Add(aNode);
        }
    }
    public static class PassageExtensions
    {
        public enum Version
        {
            One,
            Two,
        }
        public static List<Path> GetAllPathsToEnd(this Node startNode, List<Node> AllNodes, Version version)
        {
            var paths = new List<Path>();

            var AllPaths = new List<Path>() { new Path(startNode) };

            if (version == Version.One)
            {
                AllPaths = AllPaths.GetAllPathsV1(AllNodes);
                //AllPaths.Print();

                foreach (Path path in AllPaths)
                {
                    if (path.StartNode.IsStart && path.EndNode.IsEnd)
                        paths.Add(path);
                }
            }

            if (version == Version.Two)
            {
                AllPaths = AllPaths.GetAllPathsV2(AllNodes);
                foreach (Path path in AllPaths)
                {
                    AllNodes.Reset();
                    path.Nodes.Visit();

                    var hasValidStart = path.StartNode.IsStart && path.StartNode.NumVisits == 1;
                    var hasValidEnd = path.EndNode.IsEnd && path.EndNode.NumVisits == 1;
                    var numSmallTwice = path.Nodes.Where(n =>
                                                        !n.IsStart &&
                                                        !n.IsEnd &&
                                                         n.IsSmall &&
                                                         n.NumVisits > 1)
                                                  .Distinct().Count();

                    if (hasValidStart && hasValidEnd && numSmallTwice <= 1)
                        paths.Add(path);
                }

            }

            return paths;
        }
        public static List<Path> GetAllPathsV1(this List<Path> Paths, List<Node> AllNodes)
        {
            var newPaths = new List<Path>();
            foreach (var path in Paths)
            {
                AllNodes.Reset();
                path.Nodes.Visit();

                //Follow Children
                foreach (Node childNode in path.EndNode.Children)
                {
                    var CanVisit = childNode.NumVisits == 0 || !childNode.IsSmall;
                    if (CanVisit)
                    {
                        var newPathChildPath = new Path();
                        newPathChildPath.Nodes.AddRange(path.Nodes);
                        newPathChildPath.Nodes.Add(childNode.Visit());
                        newPaths.Add(newPathChildPath);
                    }
                }

                if (!path.EndNode.IsEnd)
                {
                    //Follow Parents
                    foreach (Node parentNode in path.EndNode.Parents)
                    {
                        var CanVisit = parentNode.NumVisits == 0 || !parentNode.IsSmall;
                        if (CanVisit)
                        {
                            var newParentPath = new Path();
                            newParentPath.Nodes.AddRange(path.Nodes);
                            newParentPath.Nodes.Add(parentNode.Visit());
                            newPaths.Add(newParentPath);
                        }
                    }
                }
            }

            if (newPaths.Any())
            {
                //Paths.AddRange(newPaths);
                Paths.AddRange(newPaths.GetAllPathsV1(AllNodes));
            }

            return Paths;
        }
        public static List<Path> GetAllPathsV2(this List<Path> Paths, List<Node> AllNodes)
        {
            var newPaths = new List<Path>();
            foreach (var path in Paths)
            {
                AllNodes.Reset();
                path.Nodes.Visit();

                //Follow Children
                foreach (Node childNode in path.EndNode.Children)
                {
                    var CanVisit = !childNode.IsSmall || childNode.NumVisits < 2;
                    if (CanVisit)
                    {
                        var newChildPath = new Path();
                        newChildPath.Nodes.AddRange(path.Nodes);
                        newChildPath.Nodes.Add(childNode.Visit());
                        newPaths.Add(newChildPath);
                    }
                }

                if (!path.EndNode.IsEnd)
                {
                    //Follow Parents
                    foreach (Node parentNode in path.EndNode.Parents)
                    {
                        var CanVisit = !parentNode.IsSmall || parentNode.NumVisits < 2;
                        if (CanVisit)
                        {
                            var newParentPath = new Path();
                            newParentPath.Nodes.AddRange(path.Nodes);
                            newParentPath.Nodes.Add(parentNode.Visit());
                            newPaths.Add(newParentPath);
                        }
                    }
                }
            }

            if (newPaths.Any())
            {
                var goodPaths = new List<Path>();
                foreach (var newPath in newPaths)
                {
                    AllNodes.Reset();
                    newPath.Nodes.Visit();

                    var hasValidStart = newPath.StartNode.IsStart && newPath.StartNode.NumVisits == 1;
                    var numSmallTwice = newPath.Nodes.Where(n =>
                                                           !n.IsStart &&
                                                           !n.IsEnd &&
                                                            n.IsSmall &&
                                                            n.NumVisits > 1)
                                                      .Distinct().Count();

                    if (hasValidStart && numSmallTwice <= 1)
                        goodPaths.Add(newPath);

                }

                Paths.AddRange(goodPaths.GetAllPathsV2(AllNodes));
            }

            return Paths;
        }
        public static void Reset(this List<Node> nodes)
        {
            foreach (var node in nodes)
                node.Reset();
        }
        public static void Visit(this List<Node> nodes)
        {
            foreach (var node in nodes)
                node.Visit();
        }
        public static string GetPathString(this Path path)
        {
            return String.Join(",", path.Nodes.Select(n => n.Name));
        }
        public static void Print(this List<Path> paths)
        {
            Console.WriteLine("\n--- Paths ---");
            foreach (var path in paths)
                Console.WriteLine(String.Join(",", path.Nodes.Select(n => n.Name)));
            Console.WriteLine();
        }
    }
}
