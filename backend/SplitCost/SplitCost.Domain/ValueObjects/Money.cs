namespace SplitCost.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("A moeda não pode ser vazia.");
        if (amount < 0)
            throw new ArgumentException("O valor não pode ser negativo.");
        Amount = amount;
        Currency = currency;
    }

    // Para EF Core
    private Money() { }

    public Money Add(Money other)
    {
        if (other.Currency != Currency)
            throw new ArgumentException("Moedas diferentes.");
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Divide(int divisor)
    {
        if (divisor == 0)
            throw new ArgumentException("Divisor não pode ser zero.");
        return new Money(Amount / divisor, Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}
