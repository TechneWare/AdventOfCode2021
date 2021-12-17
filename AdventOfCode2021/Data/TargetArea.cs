using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class TargetArea
    {
        private static string TestTargetAreaInput = "x=20..30, y=-10..-5";
        private static string TargetAreaInput = "x=206..250, y=-105..-57";

        public static List<(int x, int y)> Target { get; set; } = new List<(int x, int y)>();
        public static (int xMin, int xMax, int yMin, int yMax) TargetBounds { get; set; } = new(0, 0, 0, 0);
        public static void LoadData(bool TestMode)
        {
            string target = TargetAreaInput;
            if (TestMode)
                target = TestTargetAreaInput;

            var t = target.Replace(" ", "").Split(',');
            var x = t[0].Replace("x=", "").Replace("..", ",").Split(',');
            var y = t[1].Replace("y=", "").Replace("..", ",").Split(',');

            int x1 = int.Parse(x[0]);
            int x2 = int.Parse(x[1]);
            int y1 = int.Parse(y[0]);
            int y2 = int.Parse(y[1]);

            int xMin = Math.Min(x1, x2);
            int xMax = Math.Max(x1, x2);
            int yMin = Math.Min(y1, y2);
            int yMax = Math.Max(y1, y2);

            Target = new List<(int x, int y)>();
            TargetBounds = new(xMin, xMax, yMin, yMax);

            int xval = xMin;
            while (xval <= xMax)
            {
                int yval = yMin;
                while(yval <= yMax)
                {
                    Target.Add((xval, yval++));
                }
                xval++;
            }
        }
    }
}
