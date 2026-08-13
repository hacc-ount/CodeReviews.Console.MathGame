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
        private List<string[]> _PreviousGames { get; set; }

        private string _MenuChoice { get; set; }
        private string _GameSymbol { get; set; }
        private int[] _Difficulty { get; set; }

        private int _FirstNumber { get; set; }
        private int _SecondNumber { get; set; }

        private int _TrueAnswer { get; set; }

        private int _UserAnswer { get; set; }

        private string[] _Calculation { get; set;}
        private string _FormattedCalculation { get; set; }

        public Panel _CalculationDisplay { get; set; }

        public Game()
        {
            this._Random = new Random();
            this.GameRunning = true;
            this._Score = 0;
            this._PreviousGames = new List<string[]>();
            this._MenuChoice = "";
            this._Difficulty = new int[2];
            this._FirstNumber = 0;
            this._SecondNumber = 0;
            this._TrueAnswer = 0;
            this._UserAnswer = 0;
            this._Calculation = new string[5];
            this._FormattedCalculation = "";

            this._CalculationDisplay = new Panel("----------")
                .RoundedBorder();
        }

        // Get user's menu choice. Update _MenuChoice and _GameSymbol
        public void GetSetMenuOptions()
        {
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a game")
                .AddChoices("Addition", "Subtraction", "Multiplication", "Division", "Change Difficulty")
                );

            _GameSymbol = GetGameSymbol(choice);
            _MenuChoice = choice;
        }

        // Run the game for the given choice
        public void RunChoice()
        {
            // Using the users choice from the menu
            switch ( _MenuChoice )
            {
                case "Change Difficulty":
                    AskDifficulty();
                    break;
                case "Addition":
                    // Generic function for all games
                    RunGame();
                    break;
            }
        }

        public Panel DisplayCalculationPanel()
        {
            return _CalculationDisplay;
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
            string[][] askedQuestions = new string[5][];
            for (int i = 0; i < GAME_ROUNDS; i++){
                // Update numbers
                GenerateGameNumbers();
                AnsiConsole.MarkupLineInterpolated($"{_FirstNumber} {_GameSymbol} {_SecondNumber} = ");
                
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
                askedQuestions[i] = _Calculation;

                UpdateDisplay();
            } 
        }

        // Get true answer
        private int GetTrueAnswer()
        {
            switch(_GameSymbol)
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

        // Format the calculation as an array (including users answer).
        private string[] FormatCalculation()
        {
            string[] formattedCalculation = new string[5];
            formattedCalculation[0] = _FirstNumber.ToString();
            formattedCalculation[1] = _GameSymbol;
            formattedCalculation[2] = _SecondNumber.ToString();
            formattedCalculation[3] = "=";
            formattedCalculation[4] = _TrueAnswer.ToString();

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
                    formatted.AppendFormat("{0}.", temp);
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
    }
}
