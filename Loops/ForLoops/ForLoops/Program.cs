/*
 * Prime Number Finder
Objective: Create a program that finds and displays all prime numbers up to a given limit.
For loop: Loop through numbers and check for divisibility to identify prime numbers.
*/

int factors = 0;
int limit;
bool gameon = true;

while(gameon)
    { 
Console.Clear();
Console.Write("Write a limit\n");

while(!(int.TryParse(Console.ReadLine(), out limit)))
{
    Console.WriteLine("Please input a valide integer.");
}

for (int i = 2; i < limit; i++)
{
    factors = 0;
    for (int n = 1; n <= i; n++)
    {
       
        if (0 == i % n)
            
        {
            factors = factors + 1;
        }
    }
    if (factors == 2)
    {
        Console.WriteLine(i);
    }
};
Console.ReadLine();


    Console.WriteLine("Press any key to play again or q to quit.");
if ("q" == Console.ReadLine())
    {
        gameon = false;
    }

}






















