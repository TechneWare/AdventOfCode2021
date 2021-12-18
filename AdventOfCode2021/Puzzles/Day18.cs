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
            var homework = new List<SnailNum>();
            foreach (var num in numbers)
                homework.Add(num.GetSnailfishNumber());

            string solve = "";
            long mag = 0;
            if (homework.Any())
            {
                var snailAnswer = homework.First();
                solve = $"\t{snailAnswer}\n";
                if (homework.Count > 1)
                {
                    foreach (var num in homework.Skip(1))
                    {
                        solve += $"+\t{num}\n";
                        snailAnswer += num;
                        solve += $"=\t{snailAnswer}\n";
                    }
                }
                mag = snailAnswer.Magnitude();
            }

            if (TestMode || WithLogging)
                Console.WriteLine(solve);

            Part1Result = $"Mag= {mag}";
        }

        public override void Part2(bool TestMode)
        {
            var numbers = Data.Snailfish.SnailfishNumbers;
            var homework = new List<SnailNum>();
            foreach (var num in numbers)
                homework.Add(num.GetSnailfishNumber());

            long maxMag = 0;
            if (homework.Any())
            {
                for (int i = 0; i < homework.Count; i++)
                {
                    for (int j = 0; j < homework.Count; j++)
                    {
                        if (i != j)
                        {
                            var SnailfishNumber = homework[i] + homework[j];
                            maxMag = Math.Max(maxMag, SnailfishNumber.Magnitude());
                        }
                    }
                }
            }

            Part2Result = $"MaxMag= {maxMag}";
        }

        public class SnailNum
        {
            public SnailNum? Parent { get; set; }
            public SnailNum? X { get; set; } = null;
            public SnailNum? Y { get; set; } = null;
            public long? Value { get; set; }

            public SnailNum() { }
            public SnailNum(object value)
                : this(null, value) { }
            public SnailNum(SnailNum parent, object value)
            {
                this.Parent = parent;
                if (value != null &&
                    value is Int64)
                {
                    this.Value = (long)value;
                    this.X = null;
                    this.Y = null;
                }
                else if (value != null &&
                         value is JToken jToken &&
                         jToken.Type == JTokenType.Integer)
                {
                    this.Value = jToken.ToObject<int>();
                    this.X = null;
                    this.Y = null;
                }
                else if (value != null &&
                         value is JArray jArray)
                {
                    this.Value = null;
                    var v = jArray.Children().ToArray();
                    this.X = new SnailNum(this, v[0]);
                    this.Y = new SnailNum(this, v[1]);
                }
            }
            public override string ToString()
            {
                return Value.HasValue
                        ? Value.ToString()
                        : $"[{X},{Y}]";

            }
            public static SnailNum operator +(SnailNum a, SnailNum b)
            {
                var num = new SnailNum
                {
                    X = a.Copy(),
                    Y = b.Copy()
                };

                num.X.Parent = num;
                num.Y.Parent = num;
                num.Reduce();

                return num;
            }
        }
    }
    public static class SnailfishNumberExtensions
    {
        public static SnailNum GetSnailfishNumber(this string SnailfishNumber)
        {
            return new SnailNum(SnailfishNumber.FromJson<object>());
        }
        public static SnailNum Previous(this SnailNum num)
        {
            if (num.Parent == null)
                return null;

            if (num == num.Parent.X)
                return num.Parent.Previous();

            var result = num.Parent.X;
            while (result?.Y != null)
                result = result.Y;

            return result;
        }
        public static SnailNum Next(this SnailNum num)
        {
            if (num.Parent == null)
                return null;

            if (num == num.Parent.Y)
                return num.Parent.Next();

            var result = num.Parent.Y;
            while (result?.X != null)
                result = result.X;
            return result;
        }
        public static long Magnitude(this SnailNum num)
        {
            if (num.Value != null)
                return (long)num.Value;
            else if (num != null)
                return 3 * num.X.Magnitude() + 2 * num.Y.Magnitude();
            else
                return 0;
        }
        public static SnailNum Copy(this SnailNum num)
        {
            var n = new SnailNum()
            {
                Value = num.Value
            };

            if (num.X != null)
            {
                n.X = num.X.Copy();
                n.X.Parent = n;
            }

            if (num.Y != null)
            {
                n.Y = num.Y.Copy();
                n.Y.Parent = n;
            }

            return n;
        }
        public static void Explode(this SnailNum num)
        {
            var xVal = num.X?.Value;
            var yVal = num.Y?.Value;

            var xNum = num.Previous();
            if (xNum != null)
            {
                if (xNum.Value == null)
                    throw new NullReferenceException("Expected SnailNum.Value, but it was null");

                xNum.Value += xVal;
            }

            var yNum = num.Next();
            if (yNum != null)
            {
                if (yNum.Value == null)
                    throw new NullReferenceException("Expected SnailNum.Value, but it was null");

                yNum.Value += yVal;
            }

            num.Value = 0;
            num.X = null;
            num.Y = null;
        }
        public static void Split(this SnailNum num)
        {
            var xVal = num.Value / 2;
            var yVal = num.Value - xVal;
            num.Value = null;
            num.X = new SnailNum(num, xVal);
            num.Y = new SnailNum(num, yVal);
        }
        public static void Reduce(this SnailNum num)
        {
            bool isChanged = false;

            void CheckExplosion(SnailNum num, int depth)
            {
                if (num == null || isChanged)
                    return;

                if (depth >= 4 && num.X != null && num.X.Value != null)
                {
                    num.Explode();
                    isChanged = true;
                    return;
                }

                CheckExplosion(num.X, depth + 1);
                CheckExplosion(num.Y, depth + 1);
            }

            void CheckSplit(SnailNum num)
            {
                if (num == null || isChanged)
                    return;

                if (num.Value != null && num.Value >= 10)
                {
                    num.Split();
                    isChanged = true;
                    return;
                }

                CheckSplit(num.X);
                CheckSplit(num.Y);
            }

            CheckExplosion(num, 0);
            if (isChanged)
            {
                num.Reduce();
                return;
            }

            CheckSplit(num);
            if (isChanged)
                num.Reduce();
        }
    }
}
