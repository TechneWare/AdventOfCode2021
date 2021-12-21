using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Puzzles
{
    /// <summary>
    /// --- Day 20: Trench Map ---
    /// <see cref="https://adventofcode.com/2021/day/20"/>
    /// </summary>
    public class Day20 : Puzzle
    {
        public Day20()
            : base(Name: "Trench Map", DayNumber: 20) { }

        public override void Part1(bool TestMode)
        {
            Data.TrenchMap.LoadData(TestMode);

            var alg = Data.TrenchMap.Algorithm;
            var image = Data.TrenchMap.image;

            image = image.Enhance(alg, 2);
            if (WithLogging || TestMode) image.Print();

            var litPixels = image.SelectMany(p => p).Count(p => p == true);
            Part1Result = $"Pixels= {litPixels}";
        }
        public override void Part2(bool TestMode)
        {
            Data.TrenchMap.LoadData(TestMode);

            var alg = Data.TrenchMap.Algorithm;
            var image = Data.TrenchMap.image;

            image = image.Enhance(alg, 50);
            if (WithLogging) image.Print();

            var litPixels = image.SelectMany(p => p).Count(p => p == true);
            Part2Result = $"Pixels= {litPixels}";
        }
    }

    public static class TrenchMapExtensions
    {
        public static int GetPixelValue(this List<List<bool>> image, int row, int col, bool defaultValue)
        {
            //Get window bounds
            int rMin = row - 1;
            int rMax = rMin + 3;
            int cMin = col - 1;
            int cMax = cMin + 3;

            //suck out the bits
            string bits = "";
            for (int r = rMin; r < rMax; r++)
                for (int c = cMin; c < cMax; c++)
                {
                    if (r < 0 || c < 0 || r >= image.Count || c >= image[0].Count)
                        bits += defaultValue ? "1" : "0";
                    else
                        bits += image[r][c] ? "1" : "0";
                }

            //convert to integer
            return Convert.ToInt32(bits, 2);

        }
        public static List<List<bool>> Enhance(this List<List<bool>> image, List<bool> alg, int steps)
        {
            bool defValue = false; //Default value, when pixel selected is off the map
            int padding = 2; //1 = no padding; 2 or more will create more blank space around the final image

            for (int s = 0; s < steps; s++)
            {
                var newImage = new List<List<bool>>();

                //scan must be at least 1 or more pixels above and below the image bounds
                for (int r = -padding; r < image.Count + padding; r++)
                {
                    var newLine = new List<bool>();

                    //scan must be at least 1 or more pixels left and right beyond the image bounds
                    for (int c = -padding; c < image[0].Count + padding; c++)
                        newLine.Add(alg[image.GetPixelValue(r, c, defValue)]);

                    newImage.Add(newLine);
                }

                defValue = alg.GetDefaultValue(defValue);
                
                image = newImage;
            }

            return image;
        }

        /// <summary>
        /// Tests the outcome of a default value against the algorithm to properly set bits that are off the image
        /// </summary>
        /// <param name="alg">Algorithim to test</param>
        /// <param name="defValue">The currennt default value</param>
        /// <returns>boolean indicating the proper default value</returns>
        private static bool GetDefaultValue(this List<bool> alg, bool defValue)
        {
            //Test to see what the default value should be for the given algorithim
            List<bool>? dfv = new List<bool>() { defValue, defValue, defValue,
                                                 defValue, defValue, defValue,
                                                 defValue, defValue, defValue };

            int dv = Convert.ToInt32(new string(dfv.Select(d => d ? '1' : '0').ToArray()), 2);
            defValue = alg[dv]; //If 000000000 = #(true), then default should be true
            return defValue;
        }

        public static void Print(this List<List<bool>> image)
        {
            for (int r = 0; r < image.Count; r++)
            {
                for (int c = 0; c < image[r].Count; c++)
                {
                    if (image[r][c])
                        Console.Write("#");
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
        }
    }
}
