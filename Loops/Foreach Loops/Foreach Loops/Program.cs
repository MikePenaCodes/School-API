
/*
**Contact List Search**
- **Description**: Create a contact list of people (using a collection of `Contact` objects with `Name`, `Phone`, and `Email`). Use a `foreach` loop to search and display contacts by a given name or phone number.
- **Learning Focus**: Working with objects and searching within a collection.
*/
 
using Foreach_Loops;
using System.Security.Principal;
using System.Xml.Linq;


bool accountnotfound = true;

while (accountnotfound) {
    Console.Clear();
Console.WriteLine("FindContact");
Console.WriteLine("Please Insert Name or Phone number. ");
var answer = Console.ReadLine();
while (string.IsNullOrEmpty(answer))
{
    Console.WriteLine("Please insert a value.");
        answer = Console.ReadLine();

}
answer = char.ToUpper(answer[0]) + answer.Substring(1);

List<Contact> contactlist = new List<Contact>();


using (StreamReader outputFile = new(Configuration.docPath))
{
    contactlist.Clear();
    var line = outputFile.ReadLine();
    while (line != null)
    {
        string[] parts = line.Split(',').Select(p => p.Trim()).ToArray();

        var contactinfo = new Contact(int.Parse(parts[0]), parts[1], parts[2], parts[3]);

        contactlist.Add(contactinfo);
        line = outputFile.ReadLine();
    }
}


foreach (var contact in contactlist)
{
    if (contact.Name.Contains(answer) || contact.PhoneNumber.Contains(answer))
    {
        
        Console.WriteLine
            ($@"

        Contact Name

        {contact.Name}

        PhoneNumber
       
        {contact.PhoneNumber}

        Email Address

        {contact.EmailAddress}");


        accountnotfound = false;
       
    }
  
}
if (accountnotfound)
{
    Console.WriteLine("Contact Not Found. Please insert a valid name or phone number.");
}

Console.ReadLine();
if (!accountnotfound) {
    Console.WriteLine("Type any key to find a new customer or q to quit");
        if ("q" != Console.ReadLine())
        {
            accountnotfound = true;
        }
    }
}





