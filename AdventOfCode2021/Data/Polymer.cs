using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class Polymer
    {
        public static List<PolymerRule> Rules { get; set; } = new List<PolymerRule>();
        public static string Template { get; set; } = "";

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/PolymerInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/PolymerTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            var data = dataRaw.Split("\r\n").ToList();
            Rules = new List<PolymerRule>();
            foreach (var line in data)
            {
                var d = line.Split(" -> ");
                if (d.Length == 2)
                    Rules.Add(new PolymerRule() { Name = d[0], Value = d[1] });
                else
                    if(!string.IsNullOrEmpty(d[0]))
                        Template = d[0].Trim();
            }
        }
    }

    public class PolymerRule
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
