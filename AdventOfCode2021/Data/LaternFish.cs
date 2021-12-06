using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public static class LaternFish
    {
        public static List<int> Fish { get; set; }

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/LanternFishInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/LanternFishTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            Fish = dataRaw.Split(',')
                .Select(f => int.Parse(f)).ToList();
        }
    }
}
