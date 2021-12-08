using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public static class SevenSegment
    {
        public static List<string> Inputs { get; set; } = new List<string>();

        public static void LoadData(bool TestMode)
        {
            Inputs = new List<string>();
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/7SegmentInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/7SegmentTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            Inputs = dataRaw.Split("\r\n").ToList();
        }
    }
}
