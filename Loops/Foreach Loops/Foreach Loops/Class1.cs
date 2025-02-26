
using System.ComponentModel;

namespace Foreach_Loops;

public class Contact
{
    public int Id {get;}
    public string Name {get;set;}
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }


    public Contact(int id, string name, string phonenumber, string emailaddress)
    {
        Id = id;
        Name = name;
        PhoneNumber = phonenumber;
        EmailAddress = emailaddress;
    }







}

