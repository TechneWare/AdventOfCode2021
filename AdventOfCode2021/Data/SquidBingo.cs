using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public static class SquidBingo
    {
        public static List<int> Draws { get; set; }
        public static List<Board> Boards { get; set; }

        static SquidBingo()
        {
            Draws = new List<int>();
            Boards = new List<Board>();
        }
        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/SquidBingoInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/SquidBingoTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            if (!string.IsNullOrEmpty(dataRaw))
            {
                var lines = dataRaw.Replace("\r", "").Replace("  ", " ").Split("\n").ToList();
                Draws = lines[0].Split(',').Select(d => int.Parse(d)).ToList();
                lines.RemoveAt(0);

                Boards = new List<Board>();
                Board? board = null;

                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        if (board == null)
                            board = new Board();
                        else
                        {
                            Boards.Add(board);
                            board = new Board();
                        }
                    }
                    else
                    {
                        board?.Rows.Add(line.Trim().Split(' ').Select(d => new Square() { Value = int.Parse(d) }).ToList());
                    }
                }

                if (board != null)
                    Boards.Add(board);
            }
        }
    }

    [Serializable]
    public class Board
    {
        private int LastDraw = 0;
        public bool IsWinner
        {
            get
            {
                return Rows.Any(row => row.All(col => col.IsChosen)) ||
                       Cols.Any(col => col.All(row => row.IsChosen));
            }
        }
        public int Score
        {
            get
            {
                var result = 0;

                var unmarkedSum = Rows.Sum(row => row.Where(square => !square.IsChosen).Sum(square => square.Value));
                result = unmarkedSum * LastDraw;

                return result;
            }
        }
        public List<List<Square>> Rows { get; set; }
        public List<List<Square>> Cols
        {
            get
            {
                var colCount = Rows[0].Count();
                var cols = new List<List<Square>>();

                for (int i = 0; i < colCount; i++)
                    cols.Add(Rows.Select(row => row[i]).ToList());

                return cols;
            }
        }
        public Board()
        {
            Rows = new List<List<Square>>();
        }

        public void MarkSquars(int draw)
        {
            LastDraw = draw;
            foreach (var row in Rows.Select(cols => cols))
            {
                var matches = row.Where(square => square.Value == draw);
                if (matches.Any())
                    foreach (var match in matches)
                        match.IsChosen = true;
            }
        }
    }
    [Serializable]
    public class Square
    {
        public int Value { get; set; }
        public bool IsChosen { get; set; }
    }
}
