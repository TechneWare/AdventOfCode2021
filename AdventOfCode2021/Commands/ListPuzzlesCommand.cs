using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Commands
{
    public class ListPuzzlesCommand : ICommand, ICommandFactory
    {
        public string CommandName => "ListPuzzles";

        public string CommandArgs =>"";

        public string[] CommandAlternates => new string[] {};

        public string Description => "List All Puzzles";


        public ICommand MakeCommand(string[] args)
        {
            return new ListPuzzlesCommand();
        }

        public void Run()
        {
            foreach (var p in Utils.GetAllPuzzles().OrderBy(p=>p.DayNumber))
                Console.WriteLine($"Day:{p.DayNumber}\tName:{p.Name}");
        }
    }
}
