using Spectre.Console;
using GameLibrary;

Game calculator = new Game();

// Setup layout
Grid grid = new Grid();
grid.AddColumn();

grid.AddRow(calculator.DisplayCalculationPanel());

calculator.AskDifficulty();
while (calculator.GameRunning)
{   
    calculator.GetSetMenuOptions();
    calculator.RunChoice();
}



