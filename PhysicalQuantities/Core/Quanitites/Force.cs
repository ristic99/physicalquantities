using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Extensions;
using PhysicalQuantities.Core.Physics;

namespace PhysicalQuantities.Core.Quanitites;

public readonly struct Force
{
    public readonly PhysicalQuantityBase _base;

    public Force(double magnitude, UnitPrefix prefix = UnitPrefix.Base)
    {
        _base = new PhysicalQuantityBase(
            magnitude * prefix.GetMultiplier(),
            new DimensionalFormula(mass: 1, length: 1, time: -2),
            QuantityNature.Vector,
            OperationType.Direct
        );
    }

    internal Force(PhysicalQuantityBase baseValue)
    {
        _base = baseValue;
    }

    public double Magnitude => _base.Value;

    // ============================================
    // Svi operatori samo delegiraju na _base!
    // ============================================

    // Dot product - delegira na base
    public OperationResult Dot(Displacement displacement)
        => _base.Dot(displacement._base);

    public OperationResult Dot(Force other)
        => _base.Dot(other._base);

    // Scalar multiplication - delegira na base
    public static OperationResult operator *(Force force, double scalar)
        => force._base.ScalarMultiply(scalar);

    public static OperationResult operator *(double scalar, Force force)
        => force._base.ScalarMultiply(scalar);

    // Division by scalar
    public static OperationResult operator /(Force force, double scalar)
        => force._base.ScalarMultiply(1.0 / scalar);
}