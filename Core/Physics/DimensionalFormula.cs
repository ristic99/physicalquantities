using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace PhysicalQuantities.Core.Physics;

public readonly struct DimensionalFormula : IEquatable<DimensionalFormula>
{
    internal enum SiBaseDimension
    {
        Mass, Length, Time, ElectricCurrent, Temperature, AmountOfSubstance, LuminousIntensity
    }

    private FrozenDictionary<SiBaseDimension, int> Powers { get; }

    [SetsRequiredMembers]
    public DimensionalFormula(int mass = 0, int length = 0, int time = 0, int current = 0, int temperature = 0, int amount = 0, int luminous = 0)
    {
        var powers = new Dictionary<SiBaseDimension, int>();
        if (mass != 0) powers[SiBaseDimension.Mass] = mass;
        if (length != 0) powers[SiBaseDimension.Length] = length;
        if (time != 0) powers[SiBaseDimension.Time] = time;
        if (current != 0) powers[SiBaseDimension.ElectricCurrent] = current;
        if (temperature != 0) powers[SiBaseDimension.Temperature] = temperature;
        if (amount != 0) powers[SiBaseDimension.AmountOfSubstance] = amount;
        if (luminous != 0) powers[SiBaseDimension.LuminousIntensity] = luminous;

        Powers = powers.ToFrozenDictionary();
    }

    [SetsRequiredMembers]
    internal DimensionalFormula(Dictionary<SiBaseDimension, int> powers)
    {
        Powers = powers.Where(kvp => kvp.Value != 0).ToFrozenDictionary();
    }

    public static DimensionalFormula operator *(DimensionalFormula a, DimensionalFormula b)
        => CombineDimensions(a, b, (x, y) => x + y);

    public static DimensionalFormula operator /(DimensionalFormula a, DimensionalFormula b)
        => CombineDimensions(a, b, (x, y) => x - y);

    private static DimensionalFormula CombineDimensions(DimensionalFormula a, DimensionalFormula b,
        Func<int, int, int> operation)
    {
        var result = new Dictionary<SiBaseDimension, int>();

        foreach (var dimension in Enum.GetValues<SiBaseDimension>())
        {
            int powerA = a.Powers.GetValueOrDefault(dimension, 0);
            int powerB = b.Powers.GetValueOrDefault(dimension, 0);
            int total = operation(powerA, powerB);
            if (total != 0)
                result[dimension] = total;
        }

        return new DimensionalFormula(result);
    }

    public DimensionalFormula RaiseToPower(int exponent)
    {
        if (exponent == 1) return this;

        var result = new Dictionary<SiBaseDimension, int>();
        foreach (var kvp in Powers)
        {
            result[kvp.Key] = kvp.Value * exponent;
        }

        return new DimensionalFormula(result);
    }

    public bool Equals(DimensionalFormula other)
    {
        if (Powers.Count != other.Powers.Count)
            return false;

        return Powers.All(kvp => other.Powers.GetValueOrDefault(kvp.Key, 0) == kvp.Value);
    }

    public override bool Equals(object? obj) => obj is DimensionalFormula other && Equals(other);

    public override int GetHashCode()
    {
        return Powers.Count == 0 ? 0 : Powers.Aggregate(0, (hash, kvp) => hash ^ HashCode.Combine(kvp.Key, kvp.Value));
    }

    public override string ToString()
    {
        return Powers.Count == 0 ? "Dimensionless" : string.Join(" ", Powers.Select(kvp => $"{kvp.Key}{(kvp.Value == 1 ? "" : $"^{kvp.Value}")}"));
    }

    public static bool operator ==(DimensionalFormula left, DimensionalFormula right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DimensionalFormula left, DimensionalFormula right)
    {
        return !left.Equals(right);
    }
}