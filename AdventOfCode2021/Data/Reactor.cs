using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class Reactor
    {
        public static List<RebootStep> RebootInstructions { get; set; } = new List<RebootStep>();

        public static void LoadData(bool TestMode, int TestPart)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/ReactorCubesInput.txt";
            if (TestMode)
                if (TestPart == 1)
                    fileName = $@"{Environment.CurrentDirectory}/Data/Files/ReactorCubesTestInput.txt";
                else
                    fileName = $@"{Environment.CurrentDirectory}/Data/Files/ReactorCubesTestInput_Part2.txt";

            RebootInstructions.Clear();
            var dataRaw = File.ReadAllText(fileName);
            var lines = dataRaw.Split("\r\n");
            foreach (var line in lines)
            {

                bool TurnOn = false;
                var s = line.Split(' ');

                if (s[0] == "on")
                    TurnOn = true;

                var c = s[1].Split(',');
                var x = c[0].Replace("x=", "").Split("..");
                var y = c[1].Replace("y=", "").Split("..");
                var z = c[2].Replace("z=", "").Split("..");

                var newStep = new RebootStep(!TurnOn,
                                      new Cube(
                                        new Segment(int.Parse(x[0]), int.Parse(x[1])),
                                        new Segment(int.Parse(y[0]), int.Parse(y[1])),
                                        new Segment(int.Parse(z[0]), int.Parse(z[1]))));

                RebootInstructions.Add(newStep);
            }
        }
    }
    public record RebootStep(bool TurnOff, Cube Region);

    public record Segment(int P1, int P2)
    {
        public bool IsEmpty => P1 > P2;
        public long Length => IsEmpty ? 0 : P2 - P1 + 1;
        public Segment Intersect(Segment other) => new(Math.Max(P1, other.P1), Math.Min(P2, other.P2));
    }

    public record Cube(Segment X, Segment Y, Segment Z)
    {
        public bool IsEmpty => X.IsEmpty || Y.IsEmpty || Z.IsEmpty;
        public long Volume => X.Length * Y.Length * Z.Length;

        public Cube Intersect(Cube other) => new(X.Intersect(other.X),
                                                     Y.Intersect(other.Y),
                                                     Z.Intersect(other.Z));
    }
}
