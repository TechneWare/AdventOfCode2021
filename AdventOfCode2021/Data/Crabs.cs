using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class Crabs
    {
        public static List<int> Positions { get; set; }

        public static void LoadData(bool TestMode)
        {
            Positions = new List<int>();
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/CrabsInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/CrabsTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            Positions = dataRaw.Split(',').Select(p => int.Parse(p)).ToList();
        }
    }
}
