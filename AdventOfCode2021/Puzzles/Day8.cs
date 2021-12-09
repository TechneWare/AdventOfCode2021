using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 8: Seven Segment Search ---
    /// <see cref="https://adventofcode.com/2021/day/8"/>
    /// </summary>
    public class Day8 : Puzzle
    {
        public Day8()
            : base(Name: "--- Day 8: Seven Segment Search ---", DayNumber: 8) { }

        public override void Part1(bool TestMode)
        {
            Data.SevenSegment.LoadData(TestMode);
            var lines = Data.SevenSegment.Inputs.ToList();
            var targetSizes = new List<int> { 2, 3, 4, 7 };
            var start = DateTime.Now;
            var targets = new List<string>();
            foreach (var line in lines)
            {
                var io = line.Split(" | ");
                var input = io[0].Split(' ');
                var output = io[1].Split(' ');

                targets.AddRange(output.Where(i => targetSizes.Contains(i.Length)).Select(i => i));
            }

            var timeLapse = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay8 Part1:\tAnswer { targets.Count }\t\t{timeLapse:F8} Seconds");
        }

        public override void Part2(bool TestMode)
        {
            Data.SevenSegment.LoadData(TestMode);
            var lines = Data.SevenSegment.Inputs.ToList();
            var start = DateTime.Now;
            var outputs = new List<int>();

            foreach (var line in lines)
            {
                var io = line.Split(" | ");
                var input = io[0].Split(' ');
                var output = io[1].Split(' ');

                var decoder = new Decoder(input);
                var value = decoder.Decode(output);

                outputs.Add(value);
            }

            var result = outputs.Sum();
            var timeLapse = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay8 Part1:\tAnswer { result }\t\t{timeLapse:F8} Seconds");
        }
    }

    public class Decoder
    {
        public string[] Inputs { get; set; }
        public List<Digit> Digits { get; set; }

        public Decoder(string[] Inputs)
        {
            this.Inputs = Inputs;
            this.Digits = new List<Digit>();
            var digits = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            foreach (var digit in digits)
                Digits.Add(new Digit() { Value = digit });

            MapDigits();
        }
        private void MapDigits()
        {
            var allSegments = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            var zero = Digits.Where(d => d.Value == 0).Single();
            var one = Digits.Where(d => d.Value == 1).Single();
            var two = Digits.Where(d => d.Value == 2).Single();
            var three = Digits.Where(d => d.Value == 3).Single();
            var four = Digits.Where(d => d.Value == 4).Single();
            var five = Digits.Where(d => d.Value == 5).Single();
            var six = Digits.Where(d => d.Value == 6).Single();
            var seven = Digits.Where(d => d.Value == 7).Single();
            var eight = Digits.Where(d => d.Value == 8).Single();
            var nine = Digits.Where(d => d.Value == 9).Single();

            //Map the unique ones first
            one.Segments = string.Concat(Inputs.Where(i => i.Length == 2).Single().ToCharArray());
            seven.Segments = string.Concat(Inputs.Where(i => i.Length == 3).Single().ToCharArray());
            four.Segments = string.Concat(Inputs.Where(i => i.Length == 4).Single().ToCharArray());
            eight.Segments = Inputs.Where(i => i.Length == 7).Single();

            //Only length 6 digit that does not contain all of the 1 segments
            six.Segments = string.Concat(Inputs.Where(i => i.Length == 6
                                                      && !one.Chars.All(c => i.ToCharArray().Contains(c)))
                                                     .Single().ToCharArray());

            var a = seven.Chars.Except(one.Chars).Single(); //Is in 7 but not in 1
            var c = allSegments.Except(six.Chars).Single(); //only segment missing from 6
            var f = one.Chars.Where(ch => ch != c).Single();//other segment in 1 that is not c

            var twoThreeFive = Inputs.Where(i => i.Length == 5).ToList(); //2,3,5 are only ones that have 5 segments

            five.Segments = twoThreeFive
                .Where(i => i.ToCharArray().Contains(f)
                        && !i.ToCharArray().Contains(c)).Single(); //Has f but not c
            two.Segments = twoThreeFive
                .Where(i => i.ToCharArray().Contains(c)
                        && !i.ToCharArray().Contains(f)).Single();  //has c but not f
            three.Segments = twoThreeFive
                .Where(i => i != five.Segments
                && i != two.Segments).Single(); //only one remaining in twoThreeFive

            var b = five.Chars.Except(two.Chars).Except(one.Chars).Single(); //b unique in 5, given its not in 2 or 1
            var e = two.Chars.Except(five.Chars).Except(one.Chars).Single(); //e unique in 5, given its not in 5 or 1
            var d = four.Chars.Except(new char[] { b, c, f }).Single(); //only unfound segment from 4
            var g = allSegments.Except(new char[] { a, b, c, d, e, f }).Single(); //only missing segment

            //complete the map
            nine.Segments = String.Concat(new char[] { a, b, c, d, f, g });
            zero.Segments = String.Concat(new char[] { a, b, c, e, f, g });
        }

        public int Decode(string[] digits)
        {
            var decodedDigits = new List<Digit>();
            foreach (var digit in digits)
                decodedDigits.Add(Digits.Where(d => d.IsMatch(digit)).Single());

            return int.Parse(string.Concat(decodedDigits.Select(d => d.Value.ToString())));
        }
    }
    public class Digit
    {
        public int Value { get; set; }
        public string Segments { get; set; } = "";
        public char[] Chars { get => Segments.ToCharArray(); }

        public bool IsMatch(string input)
        {
            var inChars = string.Concat(input.ToCharArray().OrderBy(c => c));
            var thisChars = string.Concat(Chars.OrderBy(c => c));
            return inChars.SequenceEqual(thisChars);
        }
    }
}
