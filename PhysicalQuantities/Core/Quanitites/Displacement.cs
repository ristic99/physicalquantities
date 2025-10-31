using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Extensions;
using PhysicalQuantities.Core.Physics;

namespace PhysicalQuantities.Core.Quanitites;

public readonly struct Displacement
{
    public readonly PhysicalQuantityBase _base;

    public Displacement(double magnitude, UnitPrefix prefix = UnitPrefix.Base)
    {
        _base = new PhysicalQuantityBase(
            magnitude * prefix.GetMultiplier(),
            new DimensionalFormula(length: 1),
            QuantityNature.Vector,
            OperationType.Direct
        );
    }

    internal Displacement(PhysicalQuantityBase baseValue)
    {
        _base = baseValue;
    }

    public double Magnitude => _base.Value;

    // ============================================
    // Sve operacije delegiraju na base
    // ============================================

    public OperationResult Dot(Force force)
        => _base.Dot(force._base);

    public OperationResult Dot(Displacement other)
        => _base.Dot(other._base);

    public OperationResult Cross(Force force)
        => _base.Cross(force._base);

    public static OperationResult operator *(Displacement displacement, double scalar)
        => displacement._base.ScalarMultiply(scalar);

    public static OperationResult operator *(double scalar, Displacement displacement)
        => displacement._base.ScalarMultiply(scalar);

    public static OperationResult operator /(Displacement displacement, double scalar)
        => displacement._base.ScalarMultiply(1.0 / scalar);
}