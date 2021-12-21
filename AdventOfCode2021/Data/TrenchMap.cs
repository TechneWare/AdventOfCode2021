using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class TrenchMap
    {
        public static List<bool> Algorithm { get; set; } = new();
        public static List<List<bool>> image { get; set; } = new();

        public static void LoadData(bool TestMode)
        {
            Algorithm = new();
            image = new();

            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/TrenchMapInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/TrenchMapTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            var lines = dataRaw.Split("\r\n");

            int lineNum = 0;
            foreach (var line in lines)
            {
                if (lineNum == 0)
                    foreach (var bit in line)
                        Algorithm.Add(bit == '#' ? true : false);
                else if (!string.IsNullOrEmpty(line))
                {
                    var imageLine = new List<bool>();
                    foreach (var bit in line)
                        imageLine.Add(bit == '#' ? true : false);
                    image.Add(imageLine);
                }
                lineNum++;
            }
        }
    }
}
