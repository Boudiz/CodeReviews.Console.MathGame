namespace MathGame;
using System.Linq;

public static class UserInteraction
{
    private static readonly Random Rnd = new ();
    private const int AmountQuestions = 5;
    private const int TimeToAnswer = 20000; // 20s
    private static bool _random;
    
    private static async Task<int> AskQuestionAndReturnResult(Operator op)
    {
        var (num1, num2) = RNG.Generator(op);
        String question = $"What is {num1} {op.GetSymbol()} {num2} ?";

        Console.WriteLine(question);

        var cts = new CancellationTokenSource(TimeToAnswer);
        Task<int> inputTask = GetAnswer(question, cts.Token);
        // Trigger when token is invoked
        var cancelTask = Task.Delay(-1, cts.Token);
        
        var completedTask = await Task.WhenAny(cancelTask, inputTask);
        
        if (completedTask == cancelTask)
        {
            Console.WriteLine("You ran out of time, questions marked as wrong");
            Console.WriteLine("Press ENTER to move to the next question");

            // Await for ENTER, side effect also finish the ReadLines inside GetAnswer
            await inputTask;
            return 0;
        }

        int answer = await inputTask;
        return CheckResult(op.Result(num1, num2), answer);
    }

    private static async Task<int> GetAnswer(String question, CancellationToken token)
    {
        string? input = await Task.Run(Console.ReadLine);
        int answer;
        
        while (!int.TryParse(input, out answer))
        {
            // If timeout has occurred print nothing
            if (token.IsCancellationRequested)
            {
                return 0;
            }

            Console.WriteLine("Answer with a valid integer");
            Console.WriteLine(question);
        
            input = await Task.Run(Console.ReadLine);
        }

        return answer;
    }

    private static int CheckResult(int result, int answer)
    {
        return (result == answer ? 1 : 0);
    }

    // Ask questions and return amount of good answers
    public static async Task<int> AskQuestions()
    {
        var op = new Operator();
        
        Console.WriteLine("Which operand do you want (+, -, *, /, R) ? (R : random operand)");
        while (!op.TryParseSymbol(Console.ReadLine()))
        {
            Console.WriteLine("Wrong input.");
            Console.WriteLine("Which operand do you want (+, -, *, /, R) ?");
        }
        Console.WriteLine($"You have {TimeToAnswer/1000}sec to answer each question");

        _random = (op.GetSymbol() == "R");
        
        var amountGoodAnswer = 0;
        for (var i = 0; i < AmountQuestions; i++)
        {
            if (_random)
            {
                op.Randomize(Rnd);
            }
            amountGoodAnswer += await AskQuestionAndReturnResult(op);
        }

        return amountGoodAnswer;
    }

    private static string Menu(String question, String[] validOptions)
    {
        Console.WriteLine(question);
        var choice = Console.ReadLine()?.ToUpper().Trim();
        while (!validOptions.Contains(choice))
        {
            Console.WriteLine($"Answer with one of the correct letter: {string.Join(", ", validOptions)}");
            Console.WriteLine(question);
            choice = Console.ReadLine()?.ToUpper().Trim();
        }

        // choice can't be null because it has to be one of the element of validOptions
        return choice;
    }
    
    public static string Menu()
    {
        return Menu("_______________\nDo you want to play (P) or see history (H) or change difficulty (D) or quit (Q)?",
            ["P", "H", "D", "Q" ]);
    }

    public static void ShowHistory(List<(int, Difficulty)> history)
    {
        Console.WriteLine("\nGame History:");
        if (history.Count == 0)
        {
            Console.WriteLine("No games played yet");
        }

        for (var i = 0; i < history.Count; i++)
        {
            Console.WriteLine($"Game {i + 1} ({history[i].Item2}): {history[i].Item1} / {AmountQuestions}");
        }
    }

    public static string ChangeDifficulty()
    {
         return Menu($"\nThe current difficulty is {RNG.Difficulty}\nWhat difficulty do you want ? Easy (E), Medium (M), Hard (H) or Impossible (I) ?",
             ["E", "M", "H", "I"]);
    }
    
}