using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.MathTools
{
    public static class Vector
    {
        public static PointF Add(PointF p1, PointF p2)
        {
            p1.X += p2.X;
            p1.Y += p2.Y;
            return p1;
        }
        public static PointF Subtract(PointF p, PointF p2)
        {
            p.X -= p2.X;
            p.Y -= p2.Y;
            return p;
        }
        public static PointF Multiply(PointF p, double value)
        {
            return Multiply(p, (float)value);
        }
        public static PointF Multiply(PointF p, float value)
        {
            var result = new PointF(p.X, p.Y);
            result.X *= value;
            result.Y *= value;
            return result;
        }
        public static PointF? Distance(PointF position, PointF? target)
        {
            PointF? result = null;
            if (target != null)
            {
                var r = new PointF(position.X, position.Y);
                var f = (PointF)target;
                r.X -= f.X;
                r.Y -= f.Y;
                result = r;
            }

            return result;
        }
        public static double? Length(PointF? p)
        {
            double? result = null;
            if (p != null)
            {
                var p1 = (PointF)p;
                result = Math.Sqrt(Math.Pow(p1.X, 2) + Math.Pow(p1.Y, 2));
            }

            return result;
        }
        public static float? LengthF(PointF? p)
        {
            return (float?)Length(p);
        }
        public static PointF? Normalize(PointF? p)
        {
            if (p != null)
            {
                var p1 = (PointF)p;
                var length = (double)Length(p1);
                p1.X = (float)(p1.X / length);
                p1.Y = (float)(p1.Y / length);
                p = p1;
            }
            return p;
        }
        public static double Angle(PointF vector)
        {
            return Math.Atan2(vector.Y, vector.X);
        }
        public static PointF GetRotatedEndpoint(PointF startPoint, PointF forwardVector, double angle, double distance)
        {
            return GetRotatedEndpoint(startPoint, forwardVector, (float)angle, (float)distance);
        }
        public static PointF GetRotatedEndpoint(PointF startPoint, PointF forwardVector, float angle, float distance)
        {
            double radians = angle * (double)Math.PI / 180;
            var ca = (double)Math.Cos(radians);
            var sa = (double)Math.Sin(radians);

            var rotatedVector = new PointF((float)(ca * forwardVector.X - sa * forwardVector.Y), (float)(sa * forwardVector.X + ca * forwardVector.Y));
            PointF newendpoint = startPoint.Add(rotatedVector.Multiply(distance));

            return newendpoint;
        }
        public static PointF Intersection(Line line1, Line line2)
        {
            PointF start1 = line1.Start;
            PointF end1 = line1.End;
            PointF start2 = line2.Start;
            PointF end2 = line2.End;

            double denom = ((end1.X - start1.X) * (end2.Y - start2.Y)) - ((end1.Y - start1.Y) * (end2.X - start2.X));

            //  AB & CD are parallel 
            if (denom == 0)
                return PointF.Empty;

            double numer = ((start1.Y - start2.Y) * (end2.X - start2.X)) - ((start1.X - start2.X) * (end2.Y - start2.Y));

            double r = numer / denom;

            double numer2 = ((start1.Y - start2.Y) * (end1.X - start1.X)) - ((start1.X - start2.X) * (end1.Y - start1.Y));

            double s = numer2 / denom;

            if ((r < 0 || r > 1) || (s < 0 || s > 1))
                return PointF.Empty;

            // Find intersection point
            var result = new PointF();
            result.X = (float)(start1.X + (r * (end1.X - start1.X)));
            result.Y = (float)(start1.Y + (r * (end1.Y - start1.Y)));

            return result;
        }
        public static double Slope(Line line)
        {
            return (line.End.Y - line.Start.Y) / (line.End.X - line.Start.X);
        }
        public static PointF PointSlope(PointF point, double slope, double distance)
        {
            var IsVertical = Math.Abs(slope) == double.PositiveInfinity;
            if (IsVertical)
            {
                if (double.IsNegativeInfinity(slope))
                    distance = -Math.Abs(distance);

                return new PointF(point.X, (float)(point.Y + distance));
            }
            else
            {
                var r = Math.Sqrt((1 + Math.Pow(slope, 2)));
                var x = point.X + (distance / r);
                var y = point.Y + ((distance * slope) / r);
                return new PointF((float)x, (float)y);
            }
        }
    }
}
