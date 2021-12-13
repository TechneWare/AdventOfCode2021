using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Data
{
    public class PassagePathing
    {
        public static int NextId = 0;
        public static List<Node> Nodes { get; set; } = new List<Node>();

        public static void LoadData(bool TestMode)
        {
            var fileName = $@"{Environment.CurrentDirectory}/Data/Files/PassagePathInput.txt";
            if (TestMode)
                fileName = $@"{Environment.CurrentDirectory}/Data/Files/PassagePathTestInput.txt";

            var dataRaw = File.ReadAllText(fileName);
            var data = dataRaw.Split("\r\n").ToList();

            NextId = 1;
            Nodes = new List<Node>();
            foreach (var row in data)
            {
                var dataSplit = row.Split('-');

                Node? startNode = Nodes.Where(n => n.Name == dataSplit[0]).SingleOrDefault();
                Node? endNode = Nodes.Where(n => n.Name == dataSplit[1]).SingleOrDefault();

                if (startNode == null)
                {
                    startNode = new Node(dataSplit[0], NextId++);
                    Nodes.Add(startNode);
                }

                if (endNode == null)
                {
                    endNode = new Node(dataSplit[1], NextId++);
                    Nodes.Add(endNode);
                }

                if (endNode.Name == "start")
                {
                    var s = endNode;
                    var e = startNode;

                    s.Children.Add(e);
                    e.Parents.Add(s);
                }
                else if (startNode.Name == "end")
                {
                    var s = endNode;
                    var e = startNode;
                    s.Children.Add(e);
                    e.Parents.Add(s);
                }
                else
                {
                    startNode.Children.Add(endNode);
                    endNode.Parents.Add(startNode);
                }
            }
        }
    }
    public class Node
    {
        public int Id { get; internal set; }
        public List<Node> Parents { get; set; } = new List<Node>();
        public List<Node> Children { get; set; } = new List<Node>();
        public bool IsStart => Name.ToLower() == "start";
        public bool IsEnd => Name.ToLower() == "end";
        public bool IsSmall => Name.ToCharArray().All(c => char.IsLetter(c) && !char.IsUpper(c));
        public bool IsDeadEnd => !Children.Any();
        public int NumVisits { get; set; }
        public bool HasBeenVisited => NumVisits > 0;
        public string Name { get; internal set; }

        public Node(string name, int id)
        {
            Name = name;
            Id = id;
        }
        public Node Visit()
        {
            NumVisits++;
            return this;
        }
        public Node Reset()
        {
            NumVisits = 0;
            return this;
        }
    }
}
