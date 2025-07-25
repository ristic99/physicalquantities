using PhysicalQuantities.Core.Enums;

namespace PhysicalQuantities.Core.Physics;

public static class PhysicsDefinitions
{
    public static readonly Dictionary<PhysicalQuantityType, DimensionalFormula> QuantityDimensions = new()
    {
        // Dimensionless
        [PhysicalQuantityType.Dimensionless] = new DimensionalFormula(mass: 0, length: 0, time: 0, current: 0, temperature: 0, amount: 0, luminous: 0),

        // Base SI quantities
        [PhysicalQuantityType.Mass] = new DimensionalFormula(mass: 1),
        [PhysicalQuantityType.Length] = new DimensionalFormula(length: 1),
        [PhysicalQuantityType.Time] = new DimensionalFormula(time: 1),
        [PhysicalQuantityType.Current] = new DimensionalFormula(current: 1),
        [PhysicalQuantityType.Temperature] = new DimensionalFormula(temperature: 1),

        // Mechanical quantities
        [PhysicalQuantityType.Force] = new DimensionalFormula(mass: 1, length: 1, time: -2),                                    // MLT⁻² = Newton
        [PhysicalQuantityType.Energy] = new DimensionalFormula(mass: 1, length: 2, time: -2),                                   // ML²T⁻² = Joule
        [PhysicalQuantityType.Power] = new DimensionalFormula(mass: 1, length: 2, time: -3),                                    // ML²T⁻³ = Watt

        // Electrical quantities
        [PhysicalQuantityType.Voltage] = new DimensionalFormula(mass: 1, length: 2, time: -3, current: -1),                     // ML²T⁻³I⁻¹ = Volt
        [PhysicalQuantityType.Resistance] = new DimensionalFormula(mass: 1, length: 2, time: -3, current: -2),                  // ML²T⁻³I⁻² = Ohm
        [PhysicalQuantityType.Charge] = new DimensionalFormula(current: 1, time: 1),                                            // IT = Coulomb
        [PhysicalQuantityType.Capacitance] = new DimensionalFormula(mass: -1, length: -2, time: 4, current: 2),                 // M⁻¹L⁻²T⁴I² = Farad
        [PhysicalQuantityType.Inductance] = new DimensionalFormula(mass: 1, length: 2, time: -2, current: -2),                  // ML²T⁻²I⁻² = Henry
        [PhysicalQuantityType.Conductance] = new DimensionalFormula(mass: -1, length: -2, time: 3, current: 2),                 // M⁻¹L⁻²T³I² = Siemens
        [PhysicalQuantityType.ElectricField] = new DimensionalFormula(mass: 1, length: 1, time: -3, current: -1),               // MLT⁻³I⁻¹ = V/m
        [PhysicalQuantityType.MagneticField] = new DimensionalFormula(mass: 1, time: -2, current: -1),                          // MT⁻²I⁻¹ = Tesla
        [PhysicalQuantityType.MagneticFlux] = new DimensionalFormula(mass: 1, length: 2, time: -2, current: -1),                // ML²T⁻²I⁻¹ = Weber
        [PhysicalQuantityType.Frequency] = new DimensionalFormula(time: -1),                                                    // T⁻¹ = Hertz
        [PhysicalQuantityType.Resistivity] = new DimensionalFormula(mass: 1, length: 3, time: -3, current: -2),                 // ML³T⁻³I⁻² = Ohm⋅m
        [PhysicalQuantityType.Conductivity] = new DimensionalFormula(mass: -1, length: -3, time: 3, current: 2),                // M⁻¹L⁻³T³I² = S/m
    };
}