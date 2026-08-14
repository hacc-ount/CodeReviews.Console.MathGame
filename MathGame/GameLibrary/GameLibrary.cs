using Spectre.Console;
using System.Text;

namespace GameLibrary
{
    public class Game
    {
        private Random _Random { get; }
        private int GAME_ROUNDS = 5;

        public bool GameRunning { get; }
        private int _Score { get; set; }
        private List<string[,]> _PreviousGames { get; set; }

        private string _MenuChoice { get; set; }
        private string _GameSymbol { get; set; }
        private int[] _Difficulty { get; set; }

        private int _FirstNumber { get; set; }
        private int _SecondNumber { get; set; }

        private int _TrueAnswer { get; set; }

        private int _UserAnswer { get; set; }

        private string[] _Calculation { get; set; }
        private string _FormattedCalculation { get; set; }

        public Panel _CalculationDisplay { get; set; }

        public Grid _Grid { get; set; }

        public Game()
        {
            this._Random = new Random();
            this.GameRunning = true;
            this._Score = 0;
            this._PreviousGames = new List<string[,]>();
            this._MenuChoice = "";
            this._GameSymbol = "";
            this._Difficulty = new int[2];
            this._FirstNumber = 0;
            this._SecondNumber = 0;
            this._TrueAnswer = 0;
            this._UserAnswer = 0;
            this._Calculation = new string[5];
            this._FormattedCalculation = "";
            this._Grid = new Grid().AddColumn();
            this._CalculationDisplay = new Panel("----------")
                .RoundedBorder();
        }

        // Get user's menu choice. Update _MenuChoice and _GameSymbol
        public void GetSetMenuOptions()
        {
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a game")
                .AddChoices("Addition", "Subtraction", "Multiplication", "Division", "Change Difficulty", "View Past Games")
                );

            _GameSymbol = GetGameSymbol(choice);
            _MenuChoice = choice;
        }

        // Run the game for the given choice
        public void RunChoice()
        {
            // Using the users choice from the menu
            switch (_MenuChoice)
            {
                case "Addition":
                    // Generic function for all games
                    RunGame();
                    break;
                case "Change Difficulty":
                    AskDifficulty();
                    break;
                case "View Past Games":
                    ViewPastGames();
                    break;
            }
        }
        private void CreateNewGrid()
        {
            _Grid = new Grid().AddColumn();
        }
        private void DisplayCalculationPanel()
        {
            _Grid.AddRow(_CalculationDisplay);
            AnsiConsole.Write(_Grid);
        }

        // Return game symbol based on users menu choice
        private string GetGameSymbol(string choice)
        {
            switch (choice)
            {
                case "Addition":
                    return "+";
                case "Subtraction":
                    return "-";
                case "Multiplication":
                    return "*";
                case "Division":
                    return "/";
                default:
                    return "#";
            }
        }

        // Change the games difficulty
        public void AskDifficulty()
        {
            string difficulty = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a difficulty")
                .AddChoices("Easy", "Medium", "Hard")
                );
            switch (difficulty)
            {
                case "Easy":
                    _Difficulty[0] = 0;
                    _Difficulty[1] = 10;
                    break;
                case "Medium":
                    _Difficulty[0] = 0;
                    _Difficulty[1] = 100;
                    break;
                case "Hard":
                    _Difficulty[0] = -100;
                    _Difficulty[1] = 100;
                    break;
                default:
                    _Difficulty[0] = 0;
                    _Difficulty[1] = 10;
                    break;
            }
        }

        // Generate new random numbers
        private void GenerateGameNumbers()
        {
            _FirstNumber = _Random.Next(_Difficulty[0], _Difficulty[1]);
            _SecondNumber = _Random.Next(_Difficulty[0], _Difficulty[1]);

            if (_MenuChoice == "Division")
            {
                // If there's a remainder from the division
                while (_FirstNumber % _SecondNumber != 0)
                {
                    // Change the second number
                    _SecondNumber = _Random.Next(_Difficulty[0], _Difficulty[1]);
                }
            }

        }

        // Run the game rounds, and update the records to record the game.
        private void RunGame()
        {
            string[,] askedQuestions = new string[5, 3];
            for (int j = 0; j < GAME_ROUNDS; j++) {
                AnsiConsole.Clear();

                // Update numbers
                GenerateGameNumbers();
                AnsiConsole.Write(new Panel($"{_FirstNumber} {_GameSymbol} {_SecondNumber} = "));

                // Get true Answer
                try
                {
                    _TrueAnswer = GetTrueAnswer();
                }
                catch (Exception ex)
                {
                    AnsiConsole.Write(ex.Message);
                }

                // Get User Answer
                GetUserAnswer();
                // Format the calculation (with true answer)
                _Calculation = FormatCalculation();
                askedQuestions[j, 0] = string.Join("", _Calculation);
                askedQuestions[j, 1] = _Calculation[4].ToString();
                askedQuestions[j, 2] = _TrueAnswer.ToString();
                WriteFormattedString();

                AnsiConsole.Clear();
                UpdateDisplay(); // Update panel
                CreateNewGrid(); // Create new grid to get rid of the previous one
                DisplayCalculationPanel(); // Add the panel to the grid and display grid in console
                CheckCorrect(); // Display text to indicate whether answer was correct or not.

                if (j == 4)
                {
                    AnsiConsole.MarkupLine("[yellow]Game Finished![/]");
                }
                Thread.Sleep(2000);
            }
            _PreviousGames.Add(askedQuestions);

        }

        // Get true answer
        private int GetTrueAnswer()
        {
            switch (_GameSymbol)
            {
                case "+":
                    return _FirstNumber + _SecondNumber;
                case "-":
                    return _FirstNumber - _SecondNumber;
                case "*":
                    return _FirstNumber * _SecondNumber;
                case "/":
                    return _FirstNumber / _SecondNumber;
                default:
                    throw new Exception("Exception: Not able to return true answer");
            }
        }

        // Get the user's answer to the question
        private void GetUserAnswer()
        {
            int answer = AnsiConsole.Ask<int>("Answer: ");
            _UserAnswer = answer;
        }

        private void CheckCorrect()
        {
            if (_UserAnswer == _TrueAnswer)
            {
                AnsiConsole.MarkupLineInterpolated($"{_UserAnswer} is [green]Correct![/]");
            }
            else
            {
                AnsiConsole.MarkupLineInterpolated($"{_UserAnswer} is [red]Incorrect.[/]");
            }
        }

        // Format the calculation as an array (including users answer).
        private string[] FormatCalculation()
        {
            string[] formattedCalculation = new string[5];
            formattedCalculation[0] = _FirstNumber.ToString();
            formattedCalculation[1] = _GameSymbol;
            formattedCalculation[2] = _SecondNumber.ToString();
            formattedCalculation[3] = "=";
            formattedCalculation[4] = _UserAnswer.ToString();

            return formattedCalculation;
        }

        private void WriteFormattedString()
        {
            StringBuilder formatted = new StringBuilder();
            string temp = "";
            for (int i = 0; i < _Calculation.Length; i++)
            {
                if (i == 4)
                {
                    temp = _Calculation[i];
                    formatted.AppendFormat("{0}", temp);
                }
                else
                {
                    temp = _Calculation[i];
                    formatted.AppendFormat("{0} ", temp);
                }
            }
            temp = formatted.ToString();
            _FormattedCalculation = temp;
        }

        private void UpdateDisplay()
        {
            if (_UserAnswer == _TrueAnswer)
            {
                _CalculationDisplay = new Panel(_FormattedCalculation)
                    .RoundedBorder()
                    .BorderColor(Color.Green);
            }
            else
            {
                _CalculationDisplay = new Panel(_FormattedCalculation)
                    .RoundedBorder()
                    .BorderColor(Color.Red);
            }
        }

        private void ViewPastGames()
        {
            Table table = new Table()
                .RoundedBorder();
            table.AddColumn("Calculation");
            int gameCount = 0;
            foreach (var game in _PreviousGames)
            {
                gameCount++;
                string gameNumber = string.Format("[yellow]Game {0}:[/]", gameCount);
                table.AddRow(gameNumber);

                for (int i = 0; i < 5; i++)
                {
                    int userAnswer = int.Parse(game[i, 1]);
                    int actualAnswer = int.Parse(game[i, 2]);
                    
                    if (userAnswer == actualAnswer)
                    {
                        string calculation = string.Format("[green]{0}[/]", game[i, 0]);
                        table.AddRow(calculation);
                    }
                    else
                    {
                        string calculation = string.Format("[red]{0}[/]", game[i, 0]);
                        table.AddRow(calculation);
                    }
                }
                table.AddEmptyRow();
            }
            AnsiConsole.Clear();
            AnsiConsole.Write(table);
        }
    }
}
