using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static AdventOfCode2021.Puzzles.Day18;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 18: Snailfish ---
    /// <see cref="https://adventofcode.com/2021/day/18"/>
    /// </summary>
    public class Day18 : Puzzle
    {
        public Day18()
            : base(Name: "Snailfish", DayNumber: 18) { }

        public override void Part1(bool TestMode)
        {
            Data.Snailfish.LoadData(TestMode);

            var numbers = Data.Snailfish.SnailfishNumbers;
            var homework = new List<Node>();
            foreach (var num in numbers)
                homework.Add(num.GetNode());

            string solve = "";
            long mag = 0;
            if (homework.Any())
            {
                var result = homework.First();
                solve = $"\t{result}\n";
                if (homework.Count() > 1)
                {
                    foreach (var num in homework.Skip(1))
                    {
                        solve += $"+\t{num}\n";
                        result += num;
                        solve += $"=\t{result}\n";
                    }
                }
                mag = result.Magnitude();
            }

            if (TestMode || WithLogging)
                Console.WriteLine(solve);

            Part1Result = $"Mag= {mag}";
        }

        public override void Part2(bool TestMode)
        {
            var numbers = Data.Snailfish.SnailfishNumbers;
            var homework = new List<Node>();
            foreach (var num in numbers)
                homework.Add(num.GetNode());

            long maxMag = 0;
            if (homework.Any())
            {
                for(int i = 0; i < homework.Count; i++)
                {
                    for(int j=0; j < homework.Count; j++)
                    {
                        if (i != j)
                        {
                            var node = homework[i] + homework[j];
                            maxMag = Math.Max(maxMag,node.Magnitude());
                        }
                    }
                }
            }

            Part2Result = $"MaxMag= {maxMag}";
        }

        public class Node
        {
            public Node? Parent { get; set; }
            public Node? X { get; set; } = null;
            public Node? Y { get; set; } = null;
            public long? Value { get; set; }

            public Node() { }
            public Node(Node parent, object value)
            {
                this.Parent = parent;
                if (value != null && value is Int64)
                {
                    this.Value = (long)value;
                    this.X = null;
                    this.Y = null;
                }
                else if (value != null && ((JToken)value).Type == JTokenType.Integer)
                {
                    this.Value = ((JToken)value).ToObject<int>();
                    this.X = null;
                    this.Y = null;
                }
                else if (value != null && value is JArray)
                {
                    this.Value = null;
                    var v = ((JArray)value).Children().ToArray();
                    this.X = new Node(this, v[0]);
                    this.Y = new Node(this, v[1]);
                }
                else
                {
                    this.Value = null;
                    this.X = null;
                    this.Y = null;
                }
            }
            public override string ToString()
            {
                if (Value.HasValue)
                    return Value.ToString();

                return $"[{X},{Y}]";
            }
            public static Node operator +(Node a, Node b)
            {
                var node = new Node(null, null);
                node.X = a.Copy();
                node.Y = b.Copy();
                node.X.Parent = node;
                node.Y.Parent = node;
                node.Reduce();
                return node;
            }

        }
    }
    public static class NodeExtensions
    {
        public static Node GetNode(this string SnailfishNumber)
        {
            return new Node(null, SnailfishNumber.FromJson<object>());
        }
        public static Node Previous(this Node node)
        {
            if (node.Parent == null)
                return null;

            if (node == node.Parent.X)
                return node.Parent.Previous();

            var result = node.Parent.X;
            while (result.Y != null)
                result = result.Y;

            return result;
        }
        public static Node Next(this Node node)
        {
            if (node.Parent == null)
                return null;

            if (node == node.Parent.Y)
                return node.Parent.Next();

            var result = node.Parent.Y;
            while (result.X != null)
                result = result.X;
            return result;
        }
        public static long Magnitude(this Node node)
        {
            if (node.Value != null)
                return (long)node.Value;

            return 3 * node.X.Magnitude() + 2 * node.Y.Magnitude();
        }
        public static Node Copy(this Node node)
        {
            var n = new Node(null, null)
            {
                Value = node.Value
            };

            if (node.X != null)
            {
                n.X = node.X.Copy();
                n.X.Parent = n;
            }

            if (node.Y != null)
            {
                n.Y = node.Y.Copy();
                n.Y.Parent = n;
            }

            return n;
        }
        public static void Explode(this Node node)
        {
            var xVal = node.X?.Value;
            var yVal = node.Y?.Value;

            var xNode = node.Previous();
            if (xNode != null)
            {
                if (xNode.Value == null)
                    throw new NullReferenceException("Expected Node.Value, but it was null");

                xNode.Value += xVal;
            }

            var yNode = node.Next();
            if (yNode != null)
            {
                if (yNode.Value == null)
                    throw new NullReferenceException("Expected Node.Value, but it was null");

                yNode.Value += yVal;
            }

            node.Value = 0;
            node.X = null;
            node.Y = null;
        }
        public static void Split(this Node node)
        {
            var xVal = node.Value / 2;
            var yVal = node.Value - xVal;
            node.Value = null;
            node.X = new Node(node, xVal);
            node.Y = new Node(node, yVal);
        }
        public static void Reduce(this Node node)
        {
            bool isChanged = false;

            void CheckExplosion(Node node, int depth)
            {
                if (node == null || isChanged)
                    return;

                if (depth >= 4 && node.X != null && node.X.Value != null)
                {
                    node.Explode();
                    isChanged = true;
                    return;
                }

                CheckExplosion(node.X, depth + 1);
                CheckExplosion(node.Y, depth + 1);
            }

            void CheckSplit(Node node)
            {
                if (node == null || isChanged)
                    return;

                if (node.Value != null && node.Value >= 10)
                {
                    node.Split();
                    isChanged = true;
                    return;
                }

                CheckSplit(node.X);
                CheckSplit(node.Y);
            }

            CheckExplosion(node, 0);
            if (isChanged)
            {
                node.Reduce();
                return;
            }

            CheckSplit(node);
            if (isChanged)
                node.Reduce();
        }
    }
}
