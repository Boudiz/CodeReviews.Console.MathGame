using System.ComponentModel;

namespace MathGame;

public enum Operand
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Random
}

public class Operator
{
    private Operand _operand;
    
    public int Result(int a, int b) => _operand switch
    {
        Operand.Addition => a + b,
        Operand.Subtraction => a - b,
        Operand.Multiplication => a * b,
        Operand.Division => b != 0 ? a / b : throw new DivideByZeroException("Can't divide by zero"),
        Operand.Random => throw new Exception("An operator must be selected"),
        _ => throw new InvalidEnumArgumentException("Operand not recognized")
    };

    public string GetSymbol() => _operand switch
    {
        Operand.Addition => "+",
        Operand.Subtraction => "-",
        Operand.Multiplication => "*",
        Operand.Division => "/",
        Operand.Random => "R",
        _ => throw new InvalidEnumArgumentException("Operand not recognized")
    };
    
    public bool TryParseSymbol(string? input)
    {
        (var success, _operand) = input?.ToUpper().Trim() switch
        {
            "+" => (true, Operand.Addition),
            "-" => (true, Operand.Subtraction),
            "*" => (true, Operand.Multiplication),
            "/" => (true, Operand.Division),
            "R" => (true, Operand.Random),
            _ => (false, default)
        };
        return success;
    }

    public void Randomize(Random rnd)
    {
        var values = Enum.GetValues<Operand>();
        _operand = values[rnd.Next(values.Length - 1)];
    }
}
