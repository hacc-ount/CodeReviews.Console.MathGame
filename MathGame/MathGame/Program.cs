using Spectre.Console;
using GameLibrary;

Game calculator = new Game();

while (calculator.GameRunning)
{
    calculator.GetMenuOption();
    calculator.GenerateGameNumbers();
    calculator.RunChoice();

    // Generate numbers for 
}



