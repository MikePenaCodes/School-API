using Inventory.Controller;
using Inventory.Model;

public class Program
{
    public static void Main()
    {
        ItemManagement itemManagement = new ItemManagement();
        itemManagement.Start();
    }
}
