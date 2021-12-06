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
        public static float Length(PointF? p)
        {
            float result = 0;
            if (p != null)
            {
                var p1 = (PointF)p;
                result = (MathF.Sqrt(MathF.Pow(p1.X, 2) + MathF.Pow(p1.Y, 2)));
            }

            return result;
        }
        public static PointF? Normalize(PointF? p)
        {
            if (p != null)
            {
                var p1 = (PointF)p;
                var length = Length(p1);
                p1.X /= length;
                p1.Y /= length;
                p = p1;
            }
            return p;
        }
        public static float Angle(PointF vector)
        {
            return MathF.Atan2(vector.Y, vector.X);
        }
        public static PointF GetRotatedEndpoint(PointF startPoint, PointF forwardVector, float angle, float distance)
        {
            float radians = angle * MathF.PI / 180;
            var ca = MathF.Cos(radians);
            var sa = MathF.Sin(radians);

            var rotatedVector = new PointF(ca * forwardVector.X - sa * forwardVector.Y, sa * forwardVector.X + ca * forwardVector.Y);
            PointF newendpoint = startPoint.Add(rotatedVector.Multiply(distance));

            return newendpoint;
        }
        public static PointF Intersection(Line line1, Line line2)
        {
            PointF start1 = line1.Start;
            PointF end1 = line1.End;
            PointF start2 = line2.Start;
            PointF end2 = line2.End;

            float denom = ((end1.X - start1.X) * (end2.Y - start2.Y)) - ((end1.Y - start1.Y) * (end2.X - start2.X));

            //  AB & CD are parallel 
            if (denom == 0)
                return PointF.Empty;

            float numer = ((start1.Y - start2.Y) * (end2.X - start2.X)) - ((start1.X - start2.X) * (end2.Y - start2.Y));
            float r = numer / denom;
            float numer2 = ((start1.Y - start2.Y) * (end1.X - start1.X)) - ((start1.X - start2.X) * (end1.Y - start1.Y));
            float s = numer2 / denom;

            if ((r < 0 || r > 1) || (s < 0 || s > 1))
                return PointF.Empty;

            // Find intersection point
            var result = new PointF
            {
                X = start1.X + (r * (end1.X - start1.X)),
                Y = start1.Y + (r * (end1.Y - start1.Y))
            };

            return result;
        }
        public static float Slope(Line line)
        {
            return (line.End.Y - line.Start.Y) / (line.End.X - line.Start.X);
        }
        public static PointF PointSlope(PointF point, float slope, float distance)
        {
            var IsVertical = MathF.Abs(slope) == float.PositiveInfinity;
            if (IsVertical)
            {
                if (float.IsNegativeInfinity(slope))
                    distance = -MathF.Abs(distance);

                return new PointF(point.X, point.Y + distance);
            }
            else
            {
                var r = MathF.Sqrt((1 + MathF.Pow(slope, 2)));
                var x = point.X + (distance / r);
                var y = point.Y + ((distance * slope) / r);

                return new PointF(x, y);
            }
        }
    }
}
