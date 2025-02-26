
//7. 2D Array Search
//Description: Implement a program that will let you reserve a specific plane seat. 
//Program asks to assign seats on a small airplane that has 7 rows and 4 columns. after a seat has been selected by user input, an X is to appear in that spot and the new display is to be displayed
//user input letter A,B,C,D and a number.

Console.WriteLine("Welcome To United Airlines");


char[,] seats = {
// 0    1    2    3 
{ 'A', 'B', 'X', 'X' },//0
{ 'X', 'X', 'X', 'X' },//1
{ 'X', 'X', 'X', 'X' },//2
{ 'X', 'X', 'C', 'D' },//3
{ 'X', 'X', 'X', 'X' },//4
{ 'A', 'X', 'X', 'X' },//5
{ 'X', 'X', 'C', 'X' },//6
};

bool gameon = true;


for (int i = 0; i < seats.GetLength(0); i++)
{
    for (int j = 0; j < seats.GetLength(1); j++)
    {


        Console.Write(" " + seats[i, j] + " ");

    }

    Console.WriteLine();
}


while (gameon)
{

    bool notavailable = true;


foreach (int s in seats)
{
    if (s != 'X')
    {
        notavailable = false;
        break;
    }
}
if (notavailable)
    {
        Console.Clear();

        for (int i = 0; i < seats.GetLength(0); i++)
        {
            for (int j = 0; j < seats.GetLength(1); j++)
            {


                Console.Write(" " + seats[i, j] + " ");

            }

            Console.WriteLine();
        }
        Console.WriteLine("Sorry there are no more seats available.");
        Console.ReadLine();
        gameon = false;
        break;
    }
    
    
    Console.WriteLine("Please chose your seat. An X means the seat is not available.");


            Console.WriteLine("Letter.");
            string n;
            int column = 0;
            bool wrong = true;
            while (wrong)
            {
                switch (n = Console.ReadLine().ToUpper())
                {
                    case "A":
                        column = 0;
                        wrong = false;
                        break;
                    case "B":
                        column = 1;
                        wrong = false;
                        break;
                    case "C":
                        column = 2;
                        wrong = false;
                        break;
                    case "D":
                        column = 3;
                        wrong = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please choose A, B, C , or D.");
                        break;
                }
            }


            Console.WriteLine("Row Number.[0-6]");
            int row = 0;
            wrong = true;

            while (wrong)
            {
                while (!int.TryParse(Console.ReadLine(), out row))
                {
                    Console.WriteLine("Please enter a valid row number.");
                }
                if (-1 < row && row < 7)
                {
                    wrong = false;
                }
                else
                {

                    Console.WriteLine("Please enter a row number in the range of 0-6.");
                }
            }



            if (seats[row, column] == 'X')
            {
                Console.Clear();
                Console.WriteLine("Seat is not available. Please choose a new seat.");
            }


            else
            {
                seats[row, column] = 'X';
            }


            for (int i = 0; i < seats.GetLength(0); i++)
            {
                for (int j = 0; j < seats.GetLength(1); j++)
                {
                    Console.Write(" " + seats[i, j] + " ");
                }
                Console.WriteLine();
            }
}




