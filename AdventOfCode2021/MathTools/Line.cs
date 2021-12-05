using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.MathTools
{
    public class Line
    {
        public bool IsHorizontal { get => Start.X == End.X; }
        public bool IsVertical { get => Start.Y == End.Y; }
        public PointF Start { get; }
        public PointF End { get; }

        public Line()
        { }
        public Line(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }
        public Line(string x1, string y1, string x2, string y2)
        {
            Start = new PointF(float.Parse(x1), float.Parse(y1));
            End = new PointF(float.Parse(x2), float.Parse(y2));
        }
    }
}
