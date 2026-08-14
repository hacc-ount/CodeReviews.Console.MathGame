using Spectre.Console;
using GameLibrary;

Game calculator = new Game();

calculator.AskDifficulty();
while (calculator.GameRunning)
{   
    calculator.GetSetMenuOptions();
    calculator.RunChoice();
}



