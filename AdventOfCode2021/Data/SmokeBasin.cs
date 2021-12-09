using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class SmokeBasin
    {
        public static int[][] HeatMap { get; set; }

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/HeatMapInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/HeatMapTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            var lines = dataRaw.Split("\r\n");
            HeatMap = new int[lines.Length][];
            var row = 0;
            foreach(var line in lines)
                HeatMap[row++] = line.ToArray()
                    .Select(p => int.Parse(p.ToString())).ToArray();
        }
    }
}
