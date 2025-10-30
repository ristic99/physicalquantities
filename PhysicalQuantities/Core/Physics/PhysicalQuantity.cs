using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Extensions;

namespace PhysicalQuantities.Core.Physics;

/// <summary>
/// Everything derived automatically from first principles
/// </summary>
public readonly struct PhysicalQuantity : IEquatable<PhysicalQuantity>
{
    /// <summary>
    /// Everything derived automatically from first principles
    /// </summary>
    public PhysicalQuantity(double value,
        PhysicalQuantityType type,
        int exponent = 1,
        UnitPrefix prefix = UnitPrefix.Base)
    {
        Value = value * Math.Pow(prefix.GetMultiplier(), exponent);
        Type = type;
        Exponent = exponent;
        Nature = GetNatureForType(type);
    }

    private PhysicalQuantity(double value,
        PhysicalQuantityType type,
        QuantityNature nature,
        int exponent = 1)
    {
        Value = value;
        Type = type;
        Exponent = exponent;
        Nature = nature;
    }

    public double Value { get; } // Always in SI base units
    public PhysicalQuantityType Type { get; }
    public int Exponent { get; }
    public QuantityNature Nature { get; }

    public double GetValueIn(UnitPrefix prefix)
        => Value / Math.Pow(prefix.GetMultiplier(), Exponent);

    public static PhysicalQuantity operator +(PhysicalQuantity a, PhysicalQuantity b)
    {
        if (!a.IsCompatibleForAddition(b))
            throw new InvalidOperationException($"Cannot add incompatible quantities");

        return new PhysicalQuantity(a.Value + b.Value, a.Type, a.Exponent);
    }

    public static PhysicalQuantity operator -(PhysicalQuantity a, PhysicalQuantity b)
    {
        if (!a.IsCompatibleForAddition(b))
            throw new InvalidOperationException($"Cannot subtract incompatible quantities");

        return new PhysicalQuantity(a.Value - b.Value, a.Type, a.Exponent);
    }

    public static PhysicalQuantity operator *(PhysicalQuantity a, PhysicalQuantity b)
    {
        var resultValue = a.Value * b.Value;

        if (a.Type == b.Type)
        {
            var newExponent = a.Exponent + b.Exponent;
            var newType = AdjustTypeForExponent(a.Type, newExponent);
            return new PhysicalQuantity(resultValue, newType, newExponent);
        }

        var resultType = DimensionalAnalysisEngine.MultiplyQuantities(
            a.Type, a.Nature, a.Exponent,
            b.Type, b.Nature, b.Exponent,
            OperationType.ScalarMultiply  // ← Kontekst!
        );
        return new PhysicalQuantity(resultValue, resultType);
    }

    public static PhysicalQuantity operator /(PhysicalQuantity a, PhysicalQuantity b)
    {
        var resultValue = a.Value / b.Value;

        if (a.Type == b.Type)
        {
            var newExponent = a.Exponent - b.Exponent;
            var newType = AdjustTypeForExponent(a.Type, newExponent);
            return new PhysicalQuantity(resultValue, newType, newExponent);
        }

        var resultType = DimensionalAnalysisEngine.DivideQuantities(
            a.Type, a.Nature, a.Exponent,
            b.Type, b.Nature, b.Exponent,
            OperationType.ScalarMultiply  // ← Dodao sve parametre
        );
        return new PhysicalQuantity(resultValue, resultType);
    }

    public static PhysicalQuantity operator *(PhysicalQuantity a, double scalar)
        => new PhysicalQuantity(a.Value * scalar, a.Type, a.Exponent);

    public static PhysicalQuantity operator /(PhysicalQuantity a, double scalar)
        => new PhysicalQuantity(a.Value / scalar, a.Type, a.Exponent);

    // Dot product: Vector · Vector → Scalar
    public PhysicalQuantity Dot(PhysicalQuantity other)
    {
        if (Nature != QuantityNature.Vector || other.Nature != QuantityNature.Vector)
            throw new InvalidOperationException("Dot product requires two vectors");

        var resultValue = Value * other.Value; // Pojednostavljeno
        var resultType = DimensionalAnalysisEngine.MultiplyQuantities(
            Type, Nature, Exponent,
            other.Type, other.Nature, other.Exponent,
            OperationType.DotProduct  // ← Ključno!
        );

        return new PhysicalQuantity(resultValue, resultType, QuantityNature.Scalar);
    }

    // Cross product: Vector × Vector → Pseudovector
    public PhysicalQuantity Cross(PhysicalQuantity other)
    {
        if (Nature != QuantityNature.Vector || other.Nature != QuantityNature.Vector)
            throw new InvalidOperationException("Cross product requires two vectors");

        var resultValue = Value * other.Value; // Pojednostavljeno
        var resultType = DimensionalAnalysisEngine.MultiplyQuantities(
            Type, Nature, Exponent,
            other.Type, other.Nature, other.Exponent,
            OperationType.CrossProduct  // ← Ključno!
        );

        return new PhysicalQuantity(resultValue, resultType, QuantityNature.Pseudovector);
    }

    public bool IsCompatibleForAddition(PhysicalQuantity other)
        => Type == other.Type && Exponent == other.Exponent;

    public override string ToString()
        => $"{Value:G6} {Type.GetBaseSymbol()}{Exponent.GetExponentSymbol()}";

    public string ToString(UnitPrefix prefix)
    {
        var value = GetValueIn(prefix);
        var symbol = prefix.GetSymbol() + Type.GetBaseSymbol() + Exponent.GetExponentSymbol();
        return $"{value:G6} {symbol}";
    }

    public bool Equals(PhysicalQuantity other)
        => Math.Abs(Value - other.Value) < 1e-12 && Type == other.Type && Exponent == other.Exponent;

    public override bool Equals(object? obj)
        => obj is PhysicalQuantity other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Value, Type, Exponent);

    public static bool operator ==(PhysicalQuantity left, PhysicalQuantity right)
        => left.Equals(right);

    public static bool operator !=(PhysicalQuantity left, PhysicalQuantity right)
        => !left.Equals(right);

    private static PhysicalQuantityType AdjustTypeForExponent(PhysicalQuantityType type, int exponent)
        => exponent == 0 ? PhysicalQuantityType.Dimensionless : type;


    private static QuantityNature GetNatureForType(PhysicalQuantityType type)
    {
        return type switch
        {
            // Vectors
            PhysicalQuantityType.Force => QuantityNature.Vector,
            PhysicalQuantityType.ElectricField => QuantityNature.Vector,

            // Pseudovectors
            PhysicalQuantityType.MagneticField => QuantityNature.Pseudovector,

            // Everything else is Scalar (default)
            _ => QuantityNature.Scalar
        };
    }
}