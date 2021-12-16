using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class Packets
    {
        public static string[] Data { get; set; } = Array.Empty<string>();

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/PacketInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/PacketTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            Data = dataRaw.Split("\r\n");
        }
    }
}
