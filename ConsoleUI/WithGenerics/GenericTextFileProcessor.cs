using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public static class GenericTextFileProcessor
    {
        /// <summary>
        /// this method returns List of type T. that can be anything thats passed.
        /// This is generic method so when we create it we can pass in any type in those <> brackets
        /// and last is limiter. T can be anything(string,int,class..). and thats problem
        /// we limit it only to clases. where <T> : class (it can only be class, not int,string)
        /// second limitation. is you have to have ability to have empty constructor. 
        /// We will pass class when calling this method LoadFromTextFile<Person>("c:/Temp") 
        /// if we dont pass new() to allow to create T object with empty constructor it will throw error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<T> LoadFromTextFile<T>(string filePath) where T : class, new()
        {
            var lines = System.IO.File.ReadAllLines(filePath).ToList();
            //List of type T. so whatever type is passed to this method will be replaced with this T. like List<Person>
            List<T> output = new List<T>();
            //we create empty T object. with empty constructor. if new() is not passed it will through error and not allow to create
            //obj with empty constructor
            T entry = new T();
            //getting actual type of T. whether its Person,LogEntry class. and getting that class properties for that type. put them in var cols
            var cols = entry.GetType().GetProperties();

            // Checks to be sure we have at least one header row and one data row
            if (lines.Count < 2)
            {
                throw new IndexOutOfRangeException("The file was either empty or missing.");
            }

            // Splits the header into one column header per entry
            var headers = lines[0].Split(',');

            // Removes the header row from the lines so we don't
            // have to worry about skipping over that first row.
            lines.RemoveAt(0);

            foreach (var row in lines)
            {
                //loop through all lines and create new instance of T
                entry = new T();

                // Splits the row into individual columns. Now the index
                // of this row matches the index of the header so the
                // FirstName column header lines up with the FirstName
                // value in this row.
                var vals = row.Split(',');

                // Loops through each header entry so we can compare that
                // against the list of columns from reflection. Once we get
                // the matching column, we can do the "SetValue" method to 
                // set the column value for our entry variable to the vals
                // item at the same index as this particular header.
                for (var i = 0; i < headers.Length; i++)
                {
                    foreach (var col in cols)
                    {
                        //it will loop through headers until it find for example first name
                        //then setting for ex. first name value to passing val[i] that we just found.
                        //and basically converting value to column property type
                        if (col.Name == headers[i])
                        {
                            col.SetValue(entry, Convert.ChangeType(vals[i], col.PropertyType));
                        }
                    }
                }
                //for each row we are adding that entry to output. 
                output.Add(entry);
            }

            return output;
        }
        /// <summary>
        /// also generic method. When calling method have to provide <T> type. SaveToTextFile<Person>
        /// as params providing List<T> of type T. which can be List<Person>
        /// we limit T to be a class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void SaveToTextFile<T>(List<T> data, string filePath) where T : class
        {
            List<string> lines = new List<string>();
            StringBuilder line = new StringBuilder();

            if (data == null || data.Count == 0)
            {
                throw new ArgumentNullException("data", "You must populate the data parameter with at least one value.");
            }
            var cols = data[0].GetType().GetProperties();

            // Loops through each column and gets the name so it can comma 
            // separate it into the header row.
            foreach (var col in cols)
            {
                line.Append(col.Name);
                line.Append(",");
            }

            // Adds the column header entries to the first line (removing
            // the last comma from the end first).
            lines.Add(line.ToString().Substring(0, line.Length - 1));

            foreach (var row in data)
            {
                line = new StringBuilder();

                foreach (var col in cols)
                {
                    line.Append(col.GetValue(row));
                    line.Append(",");
                }

                // Adds the row to the set of lines (removing
                // the last comma from the end first).
                lines.Add(line.ToString().Substring(0, line.Length - 1));
            }

            System.IO.File.WriteAllLines(filePath, lines);
        }
    }
}
