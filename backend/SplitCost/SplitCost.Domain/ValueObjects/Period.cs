namespace SplitCost.Domain.ValueObjects;

/// <summary>
/// Representa um período de tempo, podendo ser mensal ou anual
/// </summary>
public class Period : ValueObject
{
    public enum PeriodType { Monthly, Yearly }
    public PeriodType Type { get; }

    public decimal ConversionFactor => Type == PeriodType.Yearly ? 1m / 12 : 1m;

    public Period(PeriodType type)
    {
        Type = type;
    }

    public decimal ConvertToMonthly(decimal value) => value * ConversionFactor;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
    }
}
