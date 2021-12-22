using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Commands
{
    public class RunPuzzleCommand : ICommand, ICommandFactory
    {
        public string CommandName => "RunPuzzle";

        public string CommandArgs => "[Day Number]";

        public string[] CommandAlternates => new string[] { "day", "show" };

        public string Description => "Run a Puzzle ([day number] by itself will run that day)";

        public double DayNumber { get; set; } = 0;
        public string arg { get; set; } = "";

        public bool WithLogging { get; set; } = false;

        public ICommand MakeCommand(string[] args)
        {
            ICommand cmd = new RunPuzzleCommand()
            {
                WithLogging = args.Any(a => a.ToLower() == "log")
            };

            if (args.Length > 1)
            {
                ((RunPuzzleCommand)(cmd)).arg = args[1];
                if (double.TryParse(args[1], out double dayNum))
                    ((RunPuzzleCommand)(cmd)).DayNumber = dayNum;
                else if (args[1].ToLower().StartsWith('l'))
                    ((RunPuzzleCommand)(cmd)).DayNumber = Utils.GetAllPuzzles().Max(p => p.DayNumber);
            }
            else
                cmd = new BadCommand("Usage: day [Day Number | last]");

            return cmd;
        }

        public void Run()
        {
            if (DayNumber != 0)
            {
                var puzzle = Utils.GetAllPuzzles()
                                  .Where(p => p.DayNumber == DayNumber)
                                  .SingleOrDefault();

                if (puzzle != null)
                    puzzle.Run(WithLogging);
                else
                    Console.WriteLine($"Unkown Day Number [{DayNumber}]");
            }
            else
                Console.WriteLine($"Invalid Day Number [{arg}]");
        }
    }
}
