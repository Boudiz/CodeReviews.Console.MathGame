namespace MathGame;

public static class Game
{
    private static readonly List<(int, Difficulty)> History = [];
    public static async Task Play()
    {

        Console.WriteLine("Welcome to an awesome math game !");
        string menuChoice;
        do
        {
            menuChoice = UserInteraction.Menu();
            switch (menuChoice)
            {
                case "P": History.Add((await UserInteraction.AskQuestions(), RNG.Difficulty)); break;
                case "H": UserInteraction.ShowHistory(History); break;
                case "D": RNG.SetDifficulty(UserInteraction.ChangeDifficulty()); break;
                case "Q": Console.WriteLine("Good Bye :)"); break;
            }
        } while (menuChoice != "Q");
    }
}