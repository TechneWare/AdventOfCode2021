﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Commands
{
    public class ClearCommand : ICommand, ICommandFactory
    {
        public string CommandName => "Clear";

        public string CommandArgs => "";

        public string[] CommandAlternates => new string[] { "cls" };

        public string Description => "Clears the screen";
        public bool WithLogging { get; set; } = false;
        public ICommand MakeCommand(string[] args)
        {
            return new ClearCommand();
        }

        public void Run()
        {
            Console.Clear();
        }
    }
}
