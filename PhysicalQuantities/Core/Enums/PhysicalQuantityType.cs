namespace PhysicalQuantities.Core.Enums;

public enum PhysicalQuantityType
{
    Dimensionless,

    // Base SI quantities
    Mass,
    Length,
    Time,
    Temperature,

    // Geometric quantities
    Area,           // m² (square meter)
    Volume,         // m³ (cubic meter)

    // Mechanical quantities
    Force,          // N (Newton)
    Energy,         // J (Joule)
    Power,          // W (Watt)
    Torque,         // N⋅m (Newton meter)

    // Electrical quantities
    Voltage,        // V (Volt)
    Current,        // A (Ampere)
    Resistance,     // Ω (Ohm)
    Charge,         // C (Coulomb)
    Capacitance,    // F (Farad)
    Inductance,     // H (Henry)
    Conductance,    // S (Siemens)
    ElectricField,  // V/m
    MagneticField,  // T (Tesla)
    MagneticFlux,   // Wb (Weber)
    Frequency,      // Hz (Hertz)
    Resistivity,    // Ω⋅m
    Conductivity,   // S/m

    // Add more types as needed
}