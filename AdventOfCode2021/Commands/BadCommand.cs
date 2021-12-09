﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Commands
{
    public class BadCommand : ICommand, ICommandFactory
    {
        public string Message { get; set; }

        public string CommandName => "BadCommand";

        public string CommandArgs => "";

        public string[] CommandAlternates => new string[] { };

        public string Description => "Internal: Used for bad commands";

        public BadCommand(string message)
        {
            Message = message;
        }

        public void Run()
        {
            Console.WriteLine($"Bad Command=> {Message}");
        }

        public ICommand MakeCommand(string[] args)
        {
            return this;
        }
    }
}
