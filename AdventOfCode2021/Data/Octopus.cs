using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class Octopus
    {
        public static int[][] Inputs { get; set; } = { };

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/OctopusInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/OctopusTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            var data = dataRaw.Split("\r\n").ToList();
            Inputs = new int[data.Count][];
            int row = 0;
            foreach (var item in data)
                Inputs[row++] = item.ToCharArray().Select(i => int.Parse(i.ToString())).ToArray();
        }
    }
}
