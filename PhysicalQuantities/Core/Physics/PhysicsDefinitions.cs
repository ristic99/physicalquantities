using PhysicalQuantities.Core.Enums;

namespace PhysicalQuantities.Core.Physics;

public static class PhysicsDefinitions
{
    // ============================================
    // Scalar quantities mapping (for PhysicalQuantity)
    // ============================================

    private static readonly Dictionary<DimensionalFormula, PhysicalQuantityType> ScalarRegistry = new()
    {
        // Dimensionless
        [new DimensionalFormula()] = PhysicalQuantityType.Dimensionless,

        // Base SI quantities (all scalar)
        [new DimensionalFormula(mass: 1)] = PhysicalQuantityType.Mass,
        [new DimensionalFormula(length: 1)] = PhysicalQuantityType.Length,
        [new DimensionalFormula(time: 1)] = PhysicalQuantityType.Time,
        [new DimensionalFormula(current: 1)] = PhysicalQuantityType.Current,
        [new DimensionalFormula(temperature: 1)] = PhysicalQuantityType.Temperature,

        // Derived scalar quantities
        [new DimensionalFormula(length: 2)] = PhysicalQuantityType.Area,  // Dodaj u enum
        [new DimensionalFormula(length: 3)] = PhysicalQuantityType.Volume, // Dodaj u enum
        [new DimensionalFormula(mass: 1, length: 2, time: -2)] = PhysicalQuantityType.Energy,  // Scalar energy
        [new DimensionalFormula(mass: 1, length: 2, time: -3)] = PhysicalQuantityType.Power,

        // Electrical quantities (scalar)
        [new DimensionalFormula(mass: 1, length: 2, time: -3, current: -1)] = PhysicalQuantityType.Voltage,
        [new DimensionalFormula(mass: 1, length: 2, time: -3, current: -2)] = PhysicalQuantityType.Resistance,
        [new DimensionalFormula(current: 1, time: 1)] = PhysicalQuantityType.Charge,
        [new DimensionalFormula(mass: -1, length: -2, time: 4, current: 2)] = PhysicalQuantityType.Capacitance,
        [new DimensionalFormula(mass: 1, length: 2, time: -2, current: -2)] = PhysicalQuantityType.Inductance,
        [new DimensionalFormula(mass: -1, length: -2, time: 3, current: 2)] = PhysicalQuantityType.Conductance,
        [new DimensionalFormula(mass: 1, length: 2, time: -2, current: -1)] = PhysicalQuantityType.MagneticFlux,
        [new DimensionalFormula(time: -1)] = PhysicalQuantityType.Frequency,
        [new DimensionalFormula(mass: 1, length: 3, time: -3, current: -2)] = PhysicalQuantityType.Resistivity,
        [new DimensionalFormula(mass: -1, length: -3, time: 3, current: 2)] = PhysicalQuantityType.Conductivity,
    };

    // ============================================
    // Public API
    // ============================================

    public static PhysicalQuantityType FindScalarQuantityType(DimensionalFormula dimensions)
    {
        if (ScalarRegistry.TryGetValue(dimensions, out var type))
            return type;

        throw new InvalidOperationException(
            $"Unknown scalar quantity with dimensions: {dimensions}");
    }

    public static DimensionalFormula GetDimensions(PhysicalQuantityType type)
    {
        var entry = ScalarRegistry.FirstOrDefault(kvp => kvp.Value == type);
        if (entry.Key != null)
            return entry.Key;

        throw new InvalidOperationException($"Unknown quantity type: {type}");
    }
}