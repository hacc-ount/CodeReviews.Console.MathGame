using Spectre.Console;

namespace GameLibrary
{
    public class Game
    {
        private Random _Random { get; set; }

        public bool GameRunning { get; }
        private int _Score { get; set; }
        private List<string> _PreviousGames { get; set; }

        private string _MenuChoice { get; set; }
        private int[] _Difficulty { get; set; }

        private int _FirstNumber { get; set; }
        private int _SecondNumber { get; set; }

        public Game()
        {
            this._Random = new Random();
            this.GameRunning = true;
            this._Score = 0;
            this._PreviousGames = new List<string>();
            this._MenuChoice = "";
            this._Difficulty = new int[2];
            this._FirstNumber = 0;
            this._SecondNumber = 0;
        }

        public int GetScore()
        {
            return _Score;
        }

        public List<string> GetPreviousGames()
        {
            return _PreviousGames;
        }

        public void GetMenuOption()
        {
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a game")
                .AddChoices("Addition", "Subtraction", "Multiplication", "Division", "Change Difficulty")
                );

            _MenuChoice = choice;
        }

        public void RunChoice()
        {
            switch ( _MenuChoice )
            {
                case "Change Difficulty":
                    AskDifficulty();
                    break;
            }
        }

        private void AskDifficulty()
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

        public void GenerateGameNumbers()
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
    }
}
