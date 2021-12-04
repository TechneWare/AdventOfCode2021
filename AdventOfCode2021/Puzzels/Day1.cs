﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzels
{
    /// <summary>
    /// --- Day 1: Sonar Sweep ---
    /// <see cref="https://adventofcode.com/2021/day/1"/>
    /// </summary>
    public class Day1: Puzzel
    {
        public Day1(){ Name = "--- Day 1: Sonar Sweep ---"; }
        public override void Part1(bool TestMode)
        {
            Data.SonarInput.TestMode = TestMode;
            var increments = 0;
            var lastValue = Data.SonarInput.Data.First();
            for (int i = 0; i < Data.SonarInput.Data.Count(); i++)
            {
                if (Data.SonarInput.Data[i] > lastValue)
                    increments++;

                lastValue = Data.SonarInput.Data[i];
            }

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay1 Part1:\tIncrements = {increments}");
        }

        public override void Part2(bool TestMode)
        {
            Data.SonarInput.TestMode = TestMode;
            var increments = 0;
            var data = Data.SonarInput.DataWindowed;

            var lastValue = data.First();
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i] > lastValue)
                    increments++;

                lastValue = data[i];
            }

            Console.WriteLine($"{(TestMode ? "Test" : "Actual")}\tDay1 Part2:\tIncrements={increments}");
        }
    }
}