﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Commands
{
    public class RunAllCommand : ICommand, ICommandFactory
    {
        public string CommandName => "RunAll";

        public string CommandArgs => "";

        public string[] CommandAlternates => new string[] { "all" };

        public string Description => "Run All Puzzles";
        public bool WithLogging { get; set; } = false;
        public ICommand MakeCommand(string[] args)
        {
            return new RunAllCommand
            {
                WithLogging = args.Any(a => a.ToLower() == "log")
            };
        }

        public void Run()
        {
            var allPuzzles = Utils.GetAllPuzzles();
            foreach (var puzzle in allPuzzles)
                puzzle.Run();
        }
    }
}
