using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{

    public enum PacketType
    {
        Literal,
        Operator
    }
    public enum OperatorType
    {
        None = 4,
        Sum = 0,
        Product = 1,
        Minimum = 2,
        Maximum = 3,
        GreaterThan = 5,
        LessThan = 6,
        EqualTo = 7
    }

    /// <summary>
    /// --- Day 16: Packet Decoder ---
    /// <see cref="https://adventofcode.com/2021/day/16"/>
    /// </summary>
    public class Day16 : Puzzle
    {
        public Day16()
            : base(Name: "Packet Decoder", DayNumber: 16) { }

        public override void Part1(bool TestMode)
        {
            Data.Packets.LoadData(TestMode);

            Part1Result = "";
            foreach (var input in Data.Packets.Data)
            {
                var reader = new PacketReader(input.ToBinary());
                Part1Result += $" {reader.GetVersionSum(reader.Packets)}";
            }

            Part1Result = $"Sums={Part1Result}";
        }

        public override void Part2(bool TestMode)
        {
            if (TestMode)
                Part2Result = "No Test Data";
            else
            {
                Data.Packets.LoadData(TestMode);

                var reader = new PacketReader(Data.Packets.Data[0].ToBinary());
                Part2Result = $" {PacketReader.GetSum(reader.Packets)}";
            }

            Part2Result = $"Sums={Part2Result}";
        }
    }

    public class PacketReader
    {
        public List<Packet> Packets { get; set; } = new List<Packet>();
        public PacketReader(string bits)
        {
            while (bits.Any(b => b == '1'))
            {
                var p = new Packet();
                bits = p.Parse(bits);

                Packets.Add(p);
            }
        }

        public long GetVersionSum(List<Packet> packets)
        {
            long sum = 0;

            foreach (var packet in packets)
            {
                sum += packet.Header.Version;
                if (packet.SubPackets.Any())
                    sum += GetVersionSum(packet.SubPackets);
            }

            return sum;
        }
        public static long GetSum(List<Packet> packets)
        {
            long sum = 0;

            if (packets.Any())
                sum = packets.Sum(p => p.Value);

            return sum;
        }
    }
    public class Packet
    {
        public PacketHeader Header { get; set; } = new PacketHeader();
        public List<Packet> SubPackets { get; set; } = new List<Packet> { };
        public long Value { get; set; }

        public string Parse(string bits)
        {
            bits = Header.Parse(bits);
            bits = GetPayload(bits);

            return bits;
        }
        private string GetPayload(string bits)
        {
            switch (Header.Type)
            {
                case PacketType.Literal:
                    bits = GetLiteral(bits);
                    break;
                case PacketType.Operator:
                    bits = GetOperator(bits);
                    break;
                default:
                    throw new Exception("Unrecognized Packet Type");
                    break;
            }

            return bits;
        }
        private string GetLiteral(string bits)
        {
            long result = 0;
            var firstBit = '0';
            do
            {
                firstBit = bits[0];
                bits = bits.Remove(0, 1);
                bits = bits.ToInt(4, out long digit);
                result = result << 4 | digit;

            } while (firstBit == '1');

            Value = result;

            return bits;
        }
        private string GetOperator(string bits)
        {
            bits = bits.ToInt(1, out long lengthTypeId);

            if (lengthTypeId == 0)
            {
                bits = bits.ToInt(15, out long subPacketLength);
                bits = bits.TakeBits((int)subPacketLength, out string subPacketBits);
                while (subPacketBits.Any(b => b == '1'))
                {
                    var p = new Packet();
                    subPacketBits = p.Parse(subPacketBits);
                    SubPackets.Add(p);
                }
                Value = this.Evaluate();
            }
            else
            {
                //Its a lengthTypeId == 1
                bits = bits.ToInt(11, out long numSubPackets);

                for (long i = 0; i < numSubPackets; i++)
                {
                    var p = new Packet();
                    bits = p.Parse(bits);
                    SubPackets.Add(p);
                }
                Value = this.Evaluate();
            }

            return bits;
        }
    }
    public class PacketHeader
    {
        private long _Version;
        private long _Type;
        
        public long Version => _Version;
        public PacketType Type => _Type == 4 ? PacketType.Literal : PacketType.Operator;
        public OperatorType Operator => (OperatorType)Enum.Parse(typeof(OperatorType), _Type.ToString());

        private string GetVersion(string bits) => bits.ToInt(3, out _Version);
        private string GetType(string bits) => bits.ToInt(3, out _Type);
        public string Parse(string bits)
        {
            bits = GetVersion(bits);
            bits = GetType(bits);

            return bits;
        }
    }

    public static class PacketExtensions
    {
        public static string ToInt(this string bits, int numBits, out long value)
        {
            var b = "";
            while (b.Length < numBits && bits.Length > 0)
            {
                b += bits[0];
                bits = bits.Remove(0, 1);
            }

            value = Convert.ToInt64(b, 2);

            return bits;
        }
        public static string TakeBits(this string bits, int numBits, out string subBits)
        {
            subBits = bits.Substring(0, numBits);
            bits = bits.Remove(0, numBits);

            return bits;
        }
        public static string ToBinary(this string Hex)
        {
            var result = "";

            foreach (var c in Hex)
                result += Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0');

            return result;
        }
        public static long Evaluate(this Packet packet)
        {
            long result = 0;

            if (packet.Header.Type == PacketType.Literal)
                result = packet.Value;
            else
            {
                switch (packet.Header.Operator)
                {
                    case OperatorType.Sum:

                        result = packet.SubPackets.Sum(p => p.Value);

                        break;
                    case OperatorType.Product:

                        if (packet.SubPackets.Count == 1)
                            result = packet.SubPackets[0].Value;
                        else if (packet.SubPackets.Count > 1)
                        {
                            result = 1;
                            foreach (var p in packet.SubPackets)
                                result *= p.Value;
                        }

                        break;
                    case OperatorType.Minimum:

                        if (packet.SubPackets.Any())
                            result = packet.SubPackets.Min(p => p.Value);

                        break;
                    case OperatorType.Maximum:

                        if (packet.SubPackets.Any())
                            result = packet.SubPackets.Max(p => p.Value);

                        break;
                    case OperatorType.GreaterThan:

                        if (packet.SubPackets.Count() != 2)
                            throw new InvalidOperationException("GreaterThan requires exactly 2 values");

                        result = packet.SubPackets[0].Value > packet.SubPackets[1].Value ? 1 : 0;

                        break;
                    case OperatorType.LessThan:

                        if (packet.SubPackets.Count() != 2)
                            throw new InvalidOperationException("LessThan requires exactly 2 values");

                        result = packet.SubPackets[0].Value < packet.SubPackets[1].Value ? 1 : 0;

                        break;
                    case OperatorType.EqualTo:

                        if (packet.SubPackets.Count() != 2)
                            throw new InvalidOperationException("EqaulTo requires exactly 2 values");

                        result = packet.SubPackets[0].Value == packet.SubPackets[1].Value ? 1 : 0;

                        break;
                    case OperatorType.None:
                    default:
                        throw new InvalidOperationException($"No Operation for Operator[{packet.Header.Operator}]");
                        break;
                }
            }

            return result;
        }
    }
}
