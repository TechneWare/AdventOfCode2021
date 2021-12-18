using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class Snailfish
    {
        public static List<string> SnailfishNumbers { get; set; } = new List<string>();

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/SnailfishInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/SnailfishTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            SnailfishNumbers = dataRaw.Split("\r\n").ToList();
        }
    }
}
