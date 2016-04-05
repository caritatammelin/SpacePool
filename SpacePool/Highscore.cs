using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpacePool
{
    class Highscore
    {
        public String Name { get; set; }
        public int Position { get; set; }
        public int Score { get; set; }

        public Highscore(String data)
        {
            var d = data.Split(' ');

            if (String.IsNullOrEmpty(data) || d.Length < 2)
                throw new ArgumentException("Invalid high score string", "data");

            this.Name = d[0];

            int num;
            if (int.TryParse(d[1], out num))
            {
                this.Score = num;
            }
            else
            {
                throw new ArgumentException("Invalid score", "data");
            }
        }

        public override string ToString()
        {
            return String.Format("{0}. {1}: {2}", this.Position, this.Name, this.Score);
        }

        static List<Highscore> ReadScoresFromFile(String path)
        {
            var scores = new List<Highscore>();

            using (StreamReader reader = new StreamReader("D:\K6089"))
            {
                String line;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    try
                    {
                        scores.Add(new Highscore(line));
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine("Invalid score at line \"{0}\": {1}", line, ex);
                    }
                }
            }

            return SortAndPositionHighscores(scores);
        }

        static List<Highscore> SortAndPositionHighscores(List<Highscore> scores)
        {
            scores = scores.OrderByDescending(s => s.Score).ToList();

            int pos = 1;

            scores.ForEach(s => s.Position = pos++);

            return scores.ToList();
        }
    }
}
