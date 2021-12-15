using AdventOfCode2021.Commands;
using AdventOfCode2021.Puzzles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Commands
{
    public static class Utils
    {
        public static IEnumerable<ICommandFactory> GetAvailableCommands()
        {
            return new ICommandFactory[]
            {
                new QuitCommand(),
                new ClearCommand(),
                new RunAllCommand(),
                new ListPuzzlesCommand(),
                new RunPuzzleCommand(),
                new WelcomeCommand(),
            };
        }

        public static IEnumerable<Puzzles.IPuzzle> GetAllPuzzles()
        {
            return new Puzzles.IPuzzle[]
            {
                new Day1(),
                new Day2(),
                new Day3(),
                new Day4(),
                new Day5_NoTrig(),
                new Day5_Trig(),
                new Day6(),
                new Day7(),
                new Day8(),
                new Day9(),
                new Day10(),
                new Day11(),
                new Day12(),
                new Day13(),
                new Day14(),
            };
        }
        public static void PrintUsage(IEnumerable<ICommandFactory> availableCommands)
        {
            Console.WriteLine("\nUsage: commandName Arguments");
            Console.WriteLine("Commands:");
            foreach (var command in availableCommands)
            {
                string alts = "";
                if (command.CommandAlternates.Any())
                    foreach (var altCommand in command.CommandAlternates)
                        alts += $" | {altCommand}";

                Console.Write($"{command.CommandName}{alts} {command.CommandArgs}".PadRight(35));
                Console.WriteLine($"-{ command.Description}");
            }
            Console.WriteLine();
        }
    }
}
