using AdventOfCode2021.MathTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public static class HydroThermal
    {
        public static IList<Line> Lines { get; set; }

        public static void LoadData(bool TestMode)
        {
            Lines = new List<Line>();
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/HydrothermalLinesInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/HydrothermalLinesTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            var rows = dataRaw.Split("\n");
            foreach (var row in rows)
            {
                var lineRaw = row.Replace(" -> ", ",").Replace("\r", "").Split(',');
                Lines.Add(new Line(lineRaw[0], lineRaw[1], lineRaw[2], lineRaw[3]));
            }
        }
    }
}
