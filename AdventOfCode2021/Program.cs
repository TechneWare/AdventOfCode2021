using AdventOfCode2021.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var availableCommands = Utils.GetAvailableCommands();
            var parser = new CommandParser(availableCommands);
                        
            parser.ParseCommand(new string[] { "Cls" }).Run();
            parser.ParseCommand(new string[] { "Welcome" }).Run();
            parser.ParseCommand(new string[] { "RunPuzzle", "Last" }).Run();

            ICommand lastCommand = null;
            do
            {
                args = GetInput().Split(' ');

                if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
                    Utils.PrintUsage(availableCommands);
                else
                {
                    ICommand? command = parser.ParseCommand(args);
                    if (command != null)
                    {
                        lastCommand = command;
                        command.Run();
                    }
                }
            } while (lastCommand == null || lastCommand.GetType() != typeof(QuitCommand));
        }
        static string GetInput()
        {
            Console.Write("$ ");
            string commandInput = Console.ReadLine();
            return commandInput;
        }
    }
}