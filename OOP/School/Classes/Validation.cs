using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Classes
{
    public class Validation
    {
        public static int Validationint(string prompt, string variable)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            int gradenumber = 0;
            while (!int.TryParse(Console.ReadLine(), out gradenumber))
            {
                Console.WriteLine($"Invalid {variable} .Please try again.");
            }
            return gradenumber;
        }

        public string ValidationId()
        {
            Console.Clear();
            Console.WriteLine("Please enter your school ID");
            string? id = Console.ReadLine();

            if (id == null)
            {
                Console.WriteLine("Invalid ID entered.");
                return null;
            }
            return id; 
        }
    }
}
