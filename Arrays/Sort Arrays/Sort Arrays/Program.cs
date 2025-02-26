//using System.Reflection.Metadata;

//Sort an Array of Dates

//Description: Given an array of dates in string format, convert them to DateTime objects and sort them in chronological order.
//Sorting Technique: Array.Sort using DateTime objects.
//Objective: Learn to handle and sort dates and times in arrays.

string[] myNumbers = { "December 26, 2024", "January 8, 2022", "March 16,2001", "June 9,1998", "August 13, 2010" };
DateTime[] myDates = new DateTime[5]; //[myNumbers.Length]
DateTime parseddate;
int index = 0;

foreach (string i in myNumbers)
{
    Console.WriteLine(i);
}



foreach (string i in myNumbers)
{
    parseddate = DateTime.Parse(i);
    myDates[index++] = parseddate;
    
}

Array.Sort(myDates);

foreach (DateTime date in myDates)
{
    Console.WriteLine(date);
}

Console.ReadLine();