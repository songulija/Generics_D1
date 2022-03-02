using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public static class OriginalTextFileProcessor
    {
        public static List<Person> LoadPeople(string filePath)
        {
            List<Person> output = new List<Person>();
            Person p;
            //read all lines from file as List
            var lines = System.IO.File.ReadAllLines(filePath).ToList();

            // Remove the header row
            lines.RemoveAt(0);
            //looping through all data and adding to List<Person>
            foreach (var line in lines)
            {
                var vals = line.Split(',');
                p = new Person();

                p.FirstName = vals[0];
                p.IsAlive = bool.Parse(vals[1]);
                p.LastName = vals[2];
                
                output.Add(p);
            }

            return output;
        }

        public static void SavePeople(List<Person> people, string filePath)
        {
            List<string> lines = new List<string>();

            // Add a header row
            lines.Add("FirstName,IsAlive,LastName");
            //adding as many lines as there are people
            foreach (var p in people)
            {
                lines.Add($"{ p.FirstName },{ p.IsAlive },{ p.LastName }");
            }
            //writing all line
            System.IO.File.WriteAllLines(filePath, lines);
        }
    }
}
