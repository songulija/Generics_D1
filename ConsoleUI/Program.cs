using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {

            //creating List that uses Generics T. That t allows us to specify which TYPE this List is about. we specify exactly what type we use
            Console.ReadLine();
            DemonstrateTextFileStorage();
            Console.WriteLine();
            Console.Write("Press enter to shut down...");
            Console.ReadLine();
        }

        private static void DemonstrateTextFileStorage()
        {
            //it creates two lists they hold different class
            List<Person> people = new List<Person>();
            List<LogEntry> logs = new List<LogEntry>();
            string peopleFile = @"C:\GenericsTesting\people.csv";
            string logFile = @"C:\GenericsTesting\logs.csv";

            //add data to both lists first
            PopulateLists(people, logs);

            //calling static GenericTextFileProcessor class method SaveToTextFile. giving it a type
            //becouse its generic method
            GenericTextFileProcessor.SaveToTextFile<Person>(people, peopleFile);
            GenericTextFileProcessor.SaveToTextFile<LogEntry>(logs, logFile);

            //loading and loggin data
            List<Person> newPeople = GenericTextFileProcessor.LoadFromTextFile<Person>(peopleFile);
            foreach (var person in newPeople)
            {
                Console.WriteLine($"{ person.FirstName } { person.LastName } (IsAlive = { person.IsAlive })");
            }

            List<LogEntry> newLogs = GenericTextFileProcessor.LoadFromTextFile<LogEntry>(logFile);
            foreach (var log in newLogs)
            {
                Console.WriteLine($"{ log.ErrorCode } { log.Message } (IsAlive = { log.TimeOfEvent.ToShortTimeString() })");
            }
        }

        private static void PopulateLists(List<Person> people, List<LogEntry> logs)
        {
            people.Add(new Person { FirstName = "Tim", LastName = "Corey" });
            people.Add(new Person { FirstName = "Sue", LastName = "Storm", IsAlive = false });
            people.Add(new Person { FirstName = "Greg", LastName = "Olsen" });

            logs.Add(new LogEntry { Message = "I blew up", ErrorCode = 9999 });
            logs.Add(new LogEntry { Message = "I'm too awesome", ErrorCode = 1337 });
            logs.Add(new LogEntry { Message = "I was tired", ErrorCode = 2222 });
        }
    }
}
