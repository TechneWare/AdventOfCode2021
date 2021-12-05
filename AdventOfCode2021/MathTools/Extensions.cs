using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.MathTools
{
    public static class Extensions
    {
        public static PointF Add(this PointF p1, PointF p2)
        {
            return Vector.Add(p1, p2);
        }
        public static PointF Subtract(this PointF p, PointF p2)
        {
            return Vector.Subtract(p, p2);
        }
        public static PointF Multiply(this PointF p, double value)
        {
            return Vector.Multiply(p, value);
        }
        public static PointF Multiply(this PointF p, float value)
        {
            return Vector.Multiply(p, value);
        }
        public static PointF? DistanceTo(this PointF position, PointF? target)
        {
            return Vector.Distance(position, target);
        }
        public static double? Length(this PointF? p)
        {
            return Vector.Length(p) * (p.Value.X / Math.Abs(p.Value.X));
        }
        public static float? LengthF(this PointF? p)
        {
            return (float)Vector.Length(p);
        }
        public static PointF? Normalize(this PointF? p)
        {
            return Vector.Normalize(p);
        }
        public static PointF? Normalize(this PointF p)
        {
            return (PointF)Vector.Normalize(p);
        }
        public static PointF ToRotatedEndpoint(this PointF startPoint, PointF forwardVector, double angle, double distance)
        {
            return Vector.GetRotatedEndpoint(startPoint, forwardVector, angle, distance);
        }
        public static PointF ToRotatedEndpoint(PointF startPoint, PointF forwardVector, float angle, float distance)
        {
            return Vector.GetRotatedEndpoint(startPoint, forwardVector, angle, distance);
        }
        public static PointF Intersection(this Line line1, Line line2)
        {
            return Vector.Intersection(line1, line2);
        }
        public static List<PointF> Points(this Line line, double step)
        {
            var result = new List<PointF>() { line.Start };

            var len = line.Length();
            var slope = line.Slope();

            var IsVertical = Math.Abs(slope) == double.PositiveInfinity;
            if (IsVertical)
            {
                len = line.End.Y - line.Start.Y;
                if (double.IsNegativeInfinity(slope))
                    step = -step;
            }
            else
                step *= (len / Math.Abs(len));

            var pos = step;
            while (Math.Abs(pos) < Math.Abs(len))
            {
                result.Add(line.PointSlope(pos));
                pos += step;
            }

            result.Add(line.End);

            return result;
        }
        public static List<PointF> PointsNoTrig(this Line line, double step)
        {
            var result = new List<PointF>();
            
            var distance = line.End.DistanceTo(line.Start);
            var xStep = Math.Abs(step) * (distance.Value.X / Math.Abs(distance.Value.X));
            var yStep = Math.Abs(step) * (distance.Value.Y / Math.Abs(distance.Value.Y));
            var xPos = (double)line.Start.X;
            var yPos = (double)line.Start.Y;

            if (xStep is double.NaN) xStep = 0;
            if (yStep is double.NaN) yStep = 0;

            while (xPos != line.End.X ||
                   yPos != line.End.Y)
            {
                result.Add(new PointF((float)xPos, (float)yPos));
                xPos += xStep;
                yPos += yStep;
            }

            result.Add(new PointF((float)xPos, (float)yPos));

            if(result.Last().X != line.End.X || result.Last().Y != line.End.Y)
                Debugger.Break();

            return result;
        }
        public static List<PointF> ToInt(this List<PointF> pointFs)
        {
            var result = new List<PointF>();
            var intPoints = pointFs.Select(p => new PointF((int)Math.Round(p.X, 0, MidpointRounding.AwayFromZero), (int)Math.Round(p.Y, 0, MidpointRounding.AwayFromZero))).ToList();
            result = intPoints.Distinct().ToList();

            return result;
        }
        public static double Length(this Line line)
        {
            var len = line.End.DistanceTo(line.Start).Length();
            double result = len.HasValue ? (double)len : 0;

            return result;
        }
        public static PointF PointSlope(this Line line, double distance)
        {
            return Vector.PointSlope(line.Start, line.Slope(), distance);
        }
        public static double Slope(this Line line)
        {
            return Vector.Slope(line);
        }
        public static double Angle(this PointF vector)
        {
            return Vector.Angle(vector);
        }
        public static double Scale(this double oldValue, double oldMin, double oldMax, double newMin, double newMax)
        {
            double newRange = newMax - newMin;
            double oldRange = oldMax - oldMin;
            return (((oldValue - oldMin) * newRange) / oldRange) + newMin;
        }
        public static float Scale(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (float)Scale((double)oldValue, (double)oldMin, (double)oldMax, (double)newMin, (double)newMax);
        }
        public static double Clamp(this double value, double minValue, double maxValue)
        {
            double result = value;

            if (value < minValue) result = minValue;
            if (value > maxValue) result = maxValue;

            return result;
        }
        public static float Clamp(this float value, float minValue, float maxValue)
        {
            return (float)Clamp((double)value, (double)minValue, (double)maxValue);
        }
    }
}
