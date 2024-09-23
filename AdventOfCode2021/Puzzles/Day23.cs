using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 23: Amphipod ---
    /// <see cref="https://adventofcode.com/2021/day/23"/>
    /// </summary>
    public class Day23 : Puzzle
    {
        public Day23()
            : base(Name: "Amphipod", DayNumber: 23)
        { }

        public override void Part1(bool TestMode)
        {
            Data.Amphipod.LoadData(TestMode);
            var result = Solve(Data.Amphipod.InitialState);

            Part1Result = $"Power = {result}";
        }

        public override void Part2(bool TestMode)
        {
            Part2Result = $"Not Implemented";
        }

        const byte AmphipodA = 0;
        const byte AmphipodD = 3;
        private static uint Solve(ulong initialState)
        {
            uint minimumCost = MinimumCost(initialState);

            var seen = new HashSet<ulong>(8192);
            var pq = new PriorityQueue<ulong, uint>(8192);
            pq.Enqueue(initialState, minimumCost * 16 + 16);

            while (pq.TryDequeue(out ulong state, out uint distance))
            {
                if (seen.Contains(state))
                    continue;

                seen.Add(state);

                uint slots = (uint)(state & 0xFFFFFFFFU);
                uint topRow = (uint)(state >> 32);

                // Try see if any amphipods in the top row can go straight to their slot
                if (TryMoveAmphipodFromTopRowToSlot(slots, topRow, out ulong newState))
                {
                    // If any amphipods moved into their slot, then there is no point considering further moves as it is an
                    // optimal decision to make
                    pq.Enqueue(newState, distance - 1);
                    continue;
                }

                bool isFinalState = true;
                for (byte amph = AmphipodA; amph <= AmphipodD; amph++)
                {
                    if (!CanMoveToSlot(amph, slots))
                    {
                        isFinalState = false;

                        uint newSlots = PopFromSlot(amph, slots, out byte newAmphipod);

                        if (CanMoveToSlot(newAmphipod, newSlots) && IsPathFromSlotToSlotClear(topRow, amph, newAmphipod))
                        {
                            pq.Enqueue((ulong)topRow << 32 | newSlots, distance - 1);
                            break;
                        }

                        uint moveCost = 16 * GetMoveCost(newAmphipod);

                        // We know that an amphipod can't stop directly outside it's spot, so we have already added moveCost * 2
                        // when determining the minimum distance, so we subtract it here to counteract that.
                        uint newDistanceStart = (uint)(distance + (amph == newAmphipod ? -moveCost * 2 : 0));

                        // Try move left
                        uint newDistance = newDistanceStart;
                        for (int i = amph + 1; i >= 0 && ((topRow & (0xFU << (4 * i))) == 0); i--)
                        {
                            if (i < (newAmphipod + 2))
                                newDistance += (i != amph + 1 && i != newAmphipod + 1 && i > 0 ? 4U : 2U) * moveCost;

                            uint newTopRow = topRow | ((8U + newAmphipod) << (4 * i));
                            pq.Enqueue((ulong)newTopRow << 32 | newSlots, newDistance);
                        }

                        // Try move right
                        newDistance = newDistanceStart;
                        for (int i = amph + 2; i < 7 && ((topRow & (0xFU << (4 * i))) == 0); i++)
                        {
                            if (i > (newAmphipod + 1))
                                newDistance += (i != amph + 2 && i != newAmphipod + 2 && i < 6 ? 4U : 2U) * moveCost;

                            uint newTopRow = topRow | ((8U + newAmphipod) << (4 * i));
                            pq.Enqueue((ulong)newTopRow << 32 | newSlots, newDistance);
                        }
                    }
                }

                if (isFinalState)
                    return distance / 16;
            }

            return 0;
        }
        
        private static uint MinimumCost(ulong state)
        {
            int totalCost = 0;
            uint slots = (uint)(state & 0xFFFFFFFFU);
            for (byte expectedAmphipod = 0; expectedAmphipod < 4; expectedAmphipod++)
            {
                byte slot = (byte)(slots & 0xFFU);
                for (int j = 0; j < 4; j++)
                {
                    if (slot == 0)
                        break;

                    byte amphipod = (byte)((slot + expectedAmphipod) & 3);

                    int distanceBetweenSlots =
                        amphipod == expectedAmphipod
                            ? 2 // We must move twice even if the slot is the same
                            : Math.Abs(amphipod - expectedAmphipod) * 2;

                    // Cost to move incorrect amphipod to space above its correct slot
                    totalCost += (j + 1 + distanceBetweenSlots) * (int)GetMoveCost(amphipod);

                    // Cost to move amphipod from above its slot into this position 
                    totalCost += (j + 1) * (int)GetMoveCost(expectedAmphipod);
                    slot >>= 2;
                }

                slots >>= 8;
            }

            return (uint)totalCost;
        }

        private static bool CanMoveToSlot(byte amphipod, uint slots) => (slots & (0xFFU << (8 * amphipod))) == 0;
        private static uint PopFromSlot(byte amphipod, uint slots, out byte newAmphipod)
        {
            int slotStart = 8 * amphipod;
            uint slot = (slots >> slotStart) & 0xFFU;
            newAmphipod = (byte)((slot + amphipod) & 3);
            uint slotMask = 0xFFU << slotStart;
            return ((slot >> 2) << slotStart) | (slots & ~slotMask);
        }
        static uint GetMoveCost(byte amph) => (uint)(((1UL | (10UL << 16) | (100UL << 32) | (1000UL << 48)) >> (amph * 16)) & 0xFFFF);
        static uint GetTopRowMask(int length, int start) => ((1U << (4 * length)) - 1) << (4 * start);
        static bool IsPathFromTopToSlotClear(uint topRow, int start, byte slot)
        {
            int leftOfSlot = slot + 1;
            int diff = start - leftOfSlot;
            uint pathMask = diff <= 0 ? GetTopRowMask(-diff, start + 1) : GetTopRowMask(diff - 1, leftOfSlot + 1);
            return (pathMask & topRow) == 0;
        }
        static bool IsPathFromSlotToSlotClear(uint topRow, byte slot1, byte slot2)
        {
            int diff = slot1 - slot2;
            uint pathMask = diff < 0 ? GetTopRowMask(-diff, slot1 + 2) : GetTopRowMask(diff, slot2 + 2);
            return (pathMask & topRow) == 0;
        }
        private static bool TryMoveAmphipodFromTopRowToSlot(uint slots, uint topRow, out ulong newState)
        {
            uint t = topRow;
            int rowIndex = 0;
            while (t != 0)
            {
                uint cell = t & 0xF;
                if (cell != 0)
                {
                    byte amph = (byte)(cell & 3);
                    if (CanMoveToSlot(amph, slots) && IsPathFromTopToSlotClear(topRow, rowIndex, amph))
                    {
                        uint newTopRow = topRow ^ (cell << (rowIndex * 4));
                        newState = ((ulong)newTopRow) << 32 | slots;
                        return true;
                    }
                }

                t >>= 4;
                rowIndex++;
            }

            newState = default;
            return false;
        }
    }
}
