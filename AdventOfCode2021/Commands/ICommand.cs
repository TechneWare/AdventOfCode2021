using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Commands
{
    public interface ICommand
    {
        bool WithLogging { get; set; }
        void Run();
    }
}
