using PhysicalQuantities.Core.Enums;

namespace PhysicalQuantities.Core.Physics;

public static class PhysicsDefinitions
{
    private record QuantityKey(
        DimensionalFormula Dimensions,
        QuantityNature Nature,
        OperationType CreationType
    );

    private static readonly Dictionary<QuantityKey, PhysicalQuantityType> Registry = new()
    {
        // ============================================
        // Dimensionless
        // ============================================
        [new QuantityKey(new DimensionalFormula(), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Dimensionless,
        [new QuantityKey(new DimensionalFormula(), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Dimensionless,

        // ============================================
        // Base SI quantities (all Scalar)
        // ============================================
        [new QuantityKey(new DimensionalFormula(mass: 1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Mass,

        [new QuantityKey(new DimensionalFormula(length: 1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Length,
        [new QuantityKey(new DimensionalFormula(length: 1), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Length,

        [new QuantityKey(new DimensionalFormula(time: 1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Time,

        [new QuantityKey(new DimensionalFormula(current: 1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Current,

        [new QuantityKey(new DimensionalFormula(temperature: 1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Temperature,

        // ============================================
        // Mechanical quantities
        // ============================================
        // Force - MLT⁻² (Vector)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 1, time: -2), QuantityNature.Vector, OperationType.ScalarMultiply)] = PhysicalQuantityType.Force,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 1, time: -2), QuantityNature.Vector, OperationType.Direct)] = PhysicalQuantityType.Force,

        // Energy - ML²T⁻² (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -2), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Energy,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -2), QuantityNature.Scalar, OperationType.DotProduct)] = PhysicalQuantityType.Energy,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -2), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Energy,

        // Power - ML²T⁻³ (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -3), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Power,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -3), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Power,

        // ============================================
        // Electrical quantities
        // ============================================
        // Voltage - ML²T⁻³I⁻¹ (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -3, current: -1), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Voltage,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -3, current: -1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Voltage,

        // Resistance - ML²T⁻³I⁻² (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -3, current: -2), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Resistance,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -3, current: -2), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Resistance,

        // Charge - IT (Scalar)
        [new QuantityKey(new DimensionalFormula(current: 1, time: 1), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Charge,
        [new QuantityKey(new DimensionalFormula(current: 1, time: 1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Charge,

        // Capacitance - M⁻¹L⁻²T⁴I² (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: -1, length: -2, time: 4, current: 2), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Capacitance,
        [new QuantityKey(new DimensionalFormula(mass: -1, length: -2, time: 4, current: 2), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Capacitance,

        // Inductance - ML²T⁻²I⁻² (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -2, current: -2), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Inductance,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -2, current: -2), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Inductance,

        // Conductance - M⁻¹L⁻²T³I² (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: -1, length: -2, time: 3, current: 2), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Conductance,
        [new QuantityKey(new DimensionalFormula(mass: -1, length: -2, time: 3, current: 2), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Conductance,

        // ElectricField - MLT⁻³I⁻¹ (Vector)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 1, time: -3, current: -1), QuantityNature.Vector, OperationType.ScalarMultiply)] = PhysicalQuantityType.ElectricField,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 1, time: -3, current: -1), QuantityNature.Vector, OperationType.Direct)] = PhysicalQuantityType.ElectricField,

        // MagneticField - MT⁻²I⁻¹ (Pseudovector)
        [new QuantityKey(new DimensionalFormula(mass: 1, time: -2, current: -1), QuantityNature.Pseudovector, OperationType.ScalarMultiply)] = PhysicalQuantityType.MagneticField,
        [new QuantityKey(new DimensionalFormula(mass: 1, time: -2, current: -1), QuantityNature.Pseudovector, OperationType.CrossProduct)] = PhysicalQuantityType.MagneticField,
        [new QuantityKey(new DimensionalFormula(mass: 1, time: -2, current: -1), QuantityNature.Pseudovector, OperationType.Direct)] = PhysicalQuantityType.MagneticField,

        // MagneticFlux - ML²T⁻²I⁻¹ (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -2, current: -1), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.MagneticFlux,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -2, current: -1), QuantityNature.Scalar, OperationType.DotProduct)] = PhysicalQuantityType.MagneticFlux,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 2, time: -2, current: -1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.MagneticFlux,

        // Frequency - T⁻¹ (Scalar)
        [new QuantityKey(new DimensionalFormula(time: -1), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Frequency,
        [new QuantityKey(new DimensionalFormula(time: -1), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Frequency,

        // Resistivity - ML³T⁻³I⁻² (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 3, time: -3, current: -2), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Resistivity,
        [new QuantityKey(new DimensionalFormula(mass: 1, length: 3, time: -3, current: -2), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Resistivity,

        // Conductivity - M⁻¹L⁻³T³I² (Scalar)
        [new QuantityKey(new DimensionalFormula(mass: -1, length: -3, time: 3, current: 2), QuantityNature.Scalar, OperationType.ScalarMultiply)] = PhysicalQuantityType.Conductivity,
        [new QuantityKey(new DimensionalFormula(mass: -1, length: -3, time: 3, current: 2), QuantityNature.Scalar, OperationType.Direct)] = PhysicalQuantityType.Conductivity,
    };

    // Public lookup metoda
    public static PhysicalQuantityType FindQuantityType(
        DimensionalFormula dimensions,
        QuantityNature nature,
        OperationType creationType)
    {
        var key = new QuantityKey(dimensions, nature, creationType);

        if (Registry.TryGetValue(key, out var type))
            return type;

        // Fallback: pokušaj sa bilo kojom creation metodom ako exact match ne postoji
        var fallbackKey = Registry.Keys
            .FirstOrDefault(k => k.Dimensions.Equals(dimensions) && k.Nature == nature);

        if (fallbackKey != null)
            return Registry[fallbackKey];

        throw new InvalidOperationException(
            $"Unknown quantity combination: {dimensions} (Nature: {nature}, CreatedBy: {creationType})");
    }

    // Helper metoda za dobijanje dimenzija (za backwards compatibility ako treba)
    public static DimensionalFormula GetDimensions(PhysicalQuantityType type)
    {
        var entry = Registry.Keys.FirstOrDefault(k => Registry[k] == type);
        if (entry != null)
            return entry.Dimensions;

        throw new InvalidOperationException($"Unknown quantity type: {type}");
    }
}