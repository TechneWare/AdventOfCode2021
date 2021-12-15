using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace AdventOfCode2021.Commands
{
    public class WelcomeCommand : ICommand, ICommandFactory
    {
        public string CommandName => "Welcome";

        public string CommandArgs => "";

        public string[] CommandAlternates => new string[] { };

        public string Description => "Displays the Welcome Message";

        public bool WithLogging { get; set; }

        public ICommand MakeCommand(string[] args)
        {
            return new WelcomeCommand();
        }

        public void Run()
        {
            Console.WriteAscii("Advent Of Code 2021", Color.Yellow);
            Console.WriteAscii(@"github.com/TechneWare", Color.Yellow);
            Console.WriteAscii("- Brian Wham", Color.WhiteSmoke);
        }
    }
}
