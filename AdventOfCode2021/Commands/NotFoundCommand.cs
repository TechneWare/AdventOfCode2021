using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Commands
{
    public class NotFoundCommand : ICommand
    {
        public bool WithLogging { get; set; } = false;
        public string Name { get; set; }
        public void Run()
        {
            Console.WriteLine($"Command Not Found: {Name}");
        }
    }
}
