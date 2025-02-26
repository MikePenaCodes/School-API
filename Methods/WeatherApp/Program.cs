Console.WriteLine("=================================");
Console.WriteLine("Welcome to the Weather App");
Console.WriteLine("=================================");

bool gameon = true;

while (gameon) { 
Console.WriteLine(@"
Please choose an option

1.Get weather for a city.
2.View weather forecast for multiple days.
3.Exit 

Enter your choice (1,2, or 3):");

int choice;
while (!int.TryParse(Console.ReadLine(), out choice) || (choice < 1 || choice > 3))
{
    Console.WriteLine("Invalid input. Please choose a valid option (1, 2, or 3).");
};
    string city = "";
switch (choice)
{
    case 1:
            Console.WriteLine("Enter the name of the city you want to check the weather for:");
            city = Console.ReadLine();
            GetWeather(city);
        break;
    case 2:
            Console.WriteLine("`Enter the name of the city you want to view the forecast for:");
            city = Console.ReadLine();
            GetForecast(city);
        break;
    case 3:
            gameon = false;
        break;
}


static void GetWeather(string city)
{
    if (city == "London")
    {
        Console.WriteLine(@"Fetching weather data for London...
-----------------------------------
- Temperature: 15°C 
- Conditions: Partly Cloudy 
- Humidity: 72% 
- Wind Speed: 10 km/h
-----------------------------------");
        }


    }
static void GetForecast(string city)
    { 
    if (city == "Paris")
        { 
        Console.WriteLine(@"-------------------------------------------------
Weather forecast for Paris: 
Day 1: 16°C, Mostly Sunny 
Day 2: 18°C, Clear Sky 
Day 3: 14°C, Light Rain 
-------------------------------------------------
");
        }
    }
}