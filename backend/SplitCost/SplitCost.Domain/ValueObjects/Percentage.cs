namespace SplitCost.Domain.ValueObjects;

public class Percentage : ValueObject
{
    public decimal Value { get; } // Ex.: 0.3 para 30%

    public Percentage(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("A margem de lucro não pode ser negativa.");
        Value = value;
    }

    // Construtor para EF Core
    private Percentage() { }

    public decimal Apply(decimal amount) => amount * (1 + Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => $"{Value * 100:F1}%";
}