
namespace AdventOfCode2021.Data
{
    public class Amphipod
    {
        public static ulong InitialState { get; set; }

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/AmphipodInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/AmphipodTestInput.txt";

            ReadOnlySpan<byte> input = File.ReadAllBytes(fileName).Where(b => b <= 127).ToArray();

            byte slotA1 = CharToAmphipodType(input[InputRowWidth * 2 + 3]);
            byte slotB1 = CharToAmphipodType(input[InputRowWidth * 2 + 5]);
            byte slotC1 = CharToAmphipodType(input[InputRowWidth * 2 + 7]);
            byte slotD1 = CharToAmphipodType(input[InputRowWidth * 2 + 9]);
            byte slotA2 = CharToAmphipodType(input[InputRowWidth * 3 + 3]);
            byte slotB2 = CharToAmphipodType(input[InputRowWidth * 3 + 5]);
            byte slotC2 = CharToAmphipodType(input[InputRowWidth * 3 + 7]);
            byte slotD2 = CharToAmphipodType(input[InputRowWidth * 3 + 9]);

            // Part 1 is simulated by assuming the bottom two slots are already filled with the correct amphipods
            InitialState =
                CreateSlot(AmphipodA, slotA1, slotA2, AmphipodA, AmphipodA) |
                ((uint)CreateSlot(AmphipodB, slotB1, slotB2, AmphipodB, AmphipodB) << 8) |
                ((uint)CreateSlot(AmphipodC, slotC1, slotC2, AmphipodC, AmphipodC) << 16) |
                ((uint)CreateSlot(AmphipodD, slotD1, slotD2, AmphipodD, AmphipodD) << 24);
        }

        const int InputRowWidth = 14;
        const byte AmphipodA = 0;
        const byte AmphipodB = 1;
        const byte AmphipodC = 2;
        const byte AmphipodD = 3;

        private static byte CharToAmphipodType(byte c) => (byte)(c - 'A');
        private static byte CreateSlot(byte expectedAmphipod, byte a1, byte a2, byte a3, byte a4)
        {
            a1 = (byte)((a1 + 4 - expectedAmphipod) & 3);
            a2 = (byte)((a2 + 4 - expectedAmphipod) & 3);
            a3 = (byte)((a3 + 4 - expectedAmphipod) & 3);
            a4 = (byte)((a4 + 4 - expectedAmphipod) & 3);
            return (byte)(a1 | (a2 << 2) | (a3 << 4) | (a4 << 6));
        }
    }
}
