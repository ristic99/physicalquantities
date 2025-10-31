using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Quanitites;

namespace PhysicalQuantities.Core.Physics;

/// <summary>
/// Represents the result of a physical operation.
/// Automatically converts to specific quantity types based on dimensions and nature.
/// </summary>
public readonly struct OperationResult
{
    private readonly PhysicalQuantityBase _base;

    public OperationResult(double value, DimensionalFormula dimensions,
                          QuantityNature nature, OperationType createdBy)
    {
        _base = new PhysicalQuantityBase(value, dimensions, nature, createdBy);
    }

    internal OperationResult(PhysicalQuantityBase baseValue)
    {
        _base = baseValue;
    }

    // Expose base for debugging/inspection
    public PhysicalQuantityBase Base => _base;

    // ============================================
    // Implicit conversions to SCALAR types
    // ============================================

    public static implicit operator Energy(OperationResult r)
    {
        var expectedDimensions = new DimensionalFormula(mass: 1, length: 2, time: -2);
        if (!r._base.Dimensions.Equals(expectedDimensions))
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Dimensions} to Energy (expected ML²T⁻²)");

        if (r._base.Nature != QuantityNature.Scalar)
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Nature} to Energy (must be Scalar)");

        return new Energy(r._base);
    }

    public static implicit operator Voltage(OperationResult r)
    {
        var expectedDimensions = new DimensionalFormula(mass: 1, length: 2, time: -3, current: -1);
        if (!r._base.Dimensions.Equals(expectedDimensions))
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Dimensions} to Voltage (expected ML²T⁻³I⁻¹)");

        if (r._base.Nature != QuantityNature.Scalar)
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Nature} to Voltage (must be Scalar)");

        return new Voltage(r._base);
    }

    public static implicit operator ElectricCurrent(OperationResult r)
    {
        var expectedDimensions = new DimensionalFormula(current: 1);
        if (!r._base.Dimensions.Equals(expectedDimensions))
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Dimensions} to ElectricCurrent (expected I)");

        if (r._base.Nature != QuantityNature.Scalar)
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Nature} to ElectricCurrent (must be Scalar)");

        return new ElectricCurrent(r._base);
    }

    public static implicit operator Resistance(OperationResult r)
    {
        var expectedDimensions = new DimensionalFormula(mass: 1, length: 2, time: -3, current: -2);
        if (!r._base.Dimensions.Equals(expectedDimensions))
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Dimensions} to Resistance (expected ML²T⁻³I⁻²)");

        if (r._base.Nature != QuantityNature.Scalar)
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Nature} to Resistance (must be Scalar)");

        return new Resistance(r._base);
    }

    // ============================================
    // Implicit conversions to VECTOR types
    // ============================================

    public static implicit operator Force(OperationResult r)
    {
        var expectedDimensions = new DimensionalFormula(mass: 1, length: 1, time: -2);
        if (!r._base.Dimensions.Equals(expectedDimensions))
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Dimensions} to Force (expected MLT⁻²)");

        if (r._base.Nature != QuantityNature.Vector)
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Nature} to Force (must be Vector)");

        return new Force(r._base);
    }

    public static implicit operator Displacement(OperationResult r)
    {
        var expectedDimensions = new DimensionalFormula(length: 1);
        if (!r._base.Dimensions.Equals(expectedDimensions))
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Dimensions} to Displacement (expected L)");

        if (r._base.Nature != QuantityNature.Vector)
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Nature} to Displacement (must be Vector)");

        return new Displacement(r._base);
    }

    // ============================================
    // Implicit conversions to PSEUDOVECTOR types
    // ============================================

    public static implicit operator Torque(OperationResult r)
    {
        var expectedDimensions = new DimensionalFormula(mass: 1, length: 2, time: -2);
        if (!r._base.Dimensions.Equals(expectedDimensions))
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Dimensions} to Torque (expected ML²T⁻²)");

        if (r._base.Nature != QuantityNature.Pseudovector)
            throw new InvalidOperationException(
                $"Cannot convert {r._base.Nature} to Torque (must be Pseudovector)");

        return new Torque(r._base);
    }

    // ============================================
    // Fallback: Convert to PhysicalQuantity (scalar)
    // ============================================

    /// <summary>
    /// Converts to PhysicalQuantity if it's a scalar quantity.
    /// This is useful for quantities that don't have explicit types yet.
    /// </summary>
    public PhysicalQuantity ToPhysicalQuantity()
    {
        if (_base.Nature != QuantityNature.Scalar)
            throw new InvalidOperationException(
                $"Cannot convert {_base.Nature} to PhysicalQuantity (only scalars supported)");

        // Find the scalar type from dimensions
        var type = PhysicsDefinitions.FindScalarQuantityType(_base.Dimensions);
        return new PhysicalQuantity(_base.Value, type);
    }

    // ============================================
    // Operators for chaining operations
    // ============================================

    public static OperationResult operator *(OperationResult a, double scalar)
    {
        return new OperationResult(
            a._base.Value * scalar,
            a._base.Dimensions,
            a._base.Nature,
            OperationType.ScalarMultiply
        );
    }

    public static OperationResult operator *(double scalar, OperationResult a)
        => a * scalar;

    public static OperationResult operator /(OperationResult a, double scalar)
    {
        return new OperationResult(
            a._base.Value / scalar,
            a._base.Dimensions,
            a._base.Nature,
            OperationType.ScalarMultiply
        );
    }

    // ============================================
    // Debug/Display
    // ============================================

    public override string ToString()
    {
        return $"{_base.Value:G6} [{_base.Dimensions}] ({_base.Nature})";
    }
}