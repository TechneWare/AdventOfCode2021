using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 10: Syntax Scoring ---
    /// <see cref="https://adventofcode.com/2021/day/9"/>
    /// </summary>
    public class Day10 : Puzzle
    {
        public Day10()
            : base(Name: "Syntax Scoring", DayNumber: 10) { }

        public override void Part1(bool TestMode)
        {
            Data.Syntax.LoadData(TestMode);

            var score = 0;
            var stack = new Stack<char>();

            foreach (var line in Data.Syntax.Inputs)
            {
                stack.Clear();
                foreach (var c in line.ToCharArray())
                {
                    if (c.IsOpener())
                        stack.Push(c);
                    else
                    {
                        var c1 = stack.Pop();
                        if (!c1.IsValidPair(c))
                            score += c.GetPoints();
                    }
                }
            }

            Part1Result = $"Score={score}";
        }

        public override void Part2(bool TestMode)
        {
            Data.Syntax.LoadData(TestMode);

            var scores = new List<long>();
            var stack = new Stack<char>();
            
            List<string> corruptLines = GetCorruptLines();
            var incompleteLines = Data.Syntax.Inputs.Except(corruptLines);

            foreach (var line in incompleteLines)
            {
                stack.Clear();
                foreach (var c in line.ToCharArray())
                    if (c.IsOpener())
                        stack.Push(c);
                    else
                        stack.Pop();

                if (stack.Any())
                    scores.Add(stack.GetCloser().GetPoints());
            }

            var midScore = scores
                        .OrderBy(s => s)
                        .Skip(scores.Count / 2)
                        .Take(1).Single();

            Part2Result = $"Score={midScore}";
        }

        private static List<string> GetCorruptLines()
        {
            var corruptLines = new List<string>();
            var stack = new Stack<char>();

            foreach (var line in Data.Syntax.Inputs)
            {
                stack.Clear();
                foreach (var c in line.ToCharArray())
                {
                    if (c.IsOpener())
                        stack.Push(c);
                    else
                    {
                        var c1 = stack.Pop();
                        if (!c1.IsValidPair(c))
                        {
                            corruptLines.Add(line);
                            break;
                        }
                    }
                }
            }

            return corruptLines;
        }
    }

    public static class SyntaxExtensions
    {
        private static readonly List<char> Openers =
            new() { '(', '[', '{', '<' };
        private static readonly List<KeyValuePair<char, char>> Closers =
            new()
            {
                new KeyValuePair<char, char>('(', ')'),
                new KeyValuePair<char, char>('[', ']'),
                new KeyValuePair<char, char>('{', '}'),
                new KeyValuePair<char, char>('<', '>'),
            };
        private static readonly List<KeyValuePair<char, int>> Points1 =
            new()
            {
                new KeyValuePair<char, int>(')', 3),
                new KeyValuePair<char, int>(']', 57),
                new KeyValuePair<char, int>('}', 1197),
                new KeyValuePair<char, int>('>', 25137)
            };
        private static readonly List<KeyValuePair<char, int>> Points2 =
            new()
            {
                new KeyValuePair<char, int>(')', 1),
                new KeyValuePair<char, int>(']', 2),
                new KeyValuePair<char, int>('}', 3),
                new KeyValuePair<char, int>('>', 4)
            };

        public static bool IsValidPair(this char c1, char c2)
        {
            return c1 == '(' && c2 == ')' ||
                   c1 == '[' && c2 == ']' ||
                   c1 == '{' && c2 == '}' ||
                   c1 == '<' && c2 == '>';
        }
        public static bool IsOpener(this char c)
        {
            return Openers.Contains(c);
        }
        public static string GetCloser(this Stack<char> stack)
        {
            string result = "";

            while (stack.Count > 0)
            {
                var c = stack.Pop();
                var closer = Closers.Where(cl => cl.Key == c).Single().Value;
                result += closer;
            }
           
            return result;
        }
        public static int GetPoints(this char c)
        {
            return Points1.Single(p => p.Key == c).Value;
        }
        public static long GetPoints(this string closer)
        {
            long score = 0;

            foreach (char c in closer)
            {
                score *= 5;
                score += Points2.Where(p => p.Key == c).Single().Value;
            }

            return score;
        }
    }
}
