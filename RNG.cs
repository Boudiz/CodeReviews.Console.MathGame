namespace MathGame;

public enum Difficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3,
    Impossible = 4
}

public static class RNG
{
    private static readonly Random Rnd = new ();
    public static Difficulty Difficulty { get; private set; } = Difficulty.Easy;
    public static (int, int) Generator(Operator op)
    {
        double amountDigit = (double)Difficulty;

        switch (op.GetSymbol())
        {
            case "+":
            case "-":
                amountDigit += 1;
                break;
            case "/":
                amountDigit /=  2; // 10^x == 10^(x/2)^2 so first number is similar in division & multiplication
                break;
        }
        
        int num1 = Rnd.Next(2, (int)Math.Ceiling(Math.Pow(10, amountDigit)) + 1);
        int num2 = Rnd.Next(2, (int)Math.Ceiling(Math.Pow(10, amountDigit)) + 1);
        
        // Get number that can be divided
        if (op.GetSymbol() == "/")
        {
            num1 = num2 * num1;
        }
        
        return (num1, num2);
    }
    
    public static void SetDifficulty(string input)
    {
        Difficulty = input.ToUpper().Trim() switch
        {
            "E" => Difficulty.Easy,
            "M" => Difficulty.Medium,
            "H" => Difficulty.Hard,
            "I" => Difficulty.Impossible,
            _ => throw new Exception("Letter not recognized")
        };
    }
    
}