using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class BeaconScanner
    {
        public static List<string> DataRaw { get; set; } = new List<string>();

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/BeaconInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/BeaconTestInput.txt";

            var d = File.ReadAllText(fileName);
            DataRaw = d.Split("\r\n").ToList();
        }
    }
}
