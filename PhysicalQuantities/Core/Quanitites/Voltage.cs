using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Extensions;
using PhysicalQuantities.Core.Physics;

namespace PhysicalQuantities.Core.Quanitites;

public readonly struct Voltage
{
    public readonly PhysicalQuantityBase _base;

    public Voltage(double value, UnitPrefix prefix = UnitPrefix.Base)
    {
        _base = new PhysicalQuantityBase(
            value * prefix.GetMultiplier(),
            new DimensionalFormula(mass: 1, length: 2, time: -3, current: -1),
            QuantityNature.Scalar,
            OperationType.Direct
        );
    }

    internal Voltage(PhysicalQuantityBase baseValue)
    {
        _base = baseValue;
    }

    public double Value => _base.Value;

    // Scalar operacije - jednostavno
    public static OperationResult operator /(Voltage voltage, ElectricCurrent current)
        => voltage._base / current._base;  // Koristi PhysicalQuantityBase operator /

    public static OperationResult operator /(Voltage voltage, Resistance resistance)
        => voltage._base / resistance._base;

    public static OperationResult operator *(Voltage voltage, double scalar)
        => voltage._base.ScalarMultiply(scalar);
}