using PhysicalQuantities.Core.Enums;

namespace PhysicalQuantities.Core.Extensions;

public static class PhysicalQuantityTypeExtensions
{
    public static string GetBaseSymbol(this PhysicalQuantityType type)
    {
        return type switch
        {
            PhysicalQuantityType.Mass => "kg",
            PhysicalQuantityType.Length => "m",
            PhysicalQuantityType.Time => "s",
            PhysicalQuantityType.Temperature => "K",
            PhysicalQuantityType.Force => "N",
            PhysicalQuantityType.Energy => "J",
            PhysicalQuantityType.Power => "W",
            PhysicalQuantityType.Voltage => "V",
            PhysicalQuantityType.Current => "A",
            PhysicalQuantityType.Resistance => "Ω",
            PhysicalQuantityType.Charge => "C",
            PhysicalQuantityType.Capacitance => "F",
            PhysicalQuantityType.Inductance => "H",
            PhysicalQuantityType.Conductance => "S",
            PhysicalQuantityType.ElectricField => "V/m",
            PhysicalQuantityType.MagneticField => "T",
            PhysicalQuantityType.MagneticFlux => "Wb",
            PhysicalQuantityType.Frequency => "Hz",
            PhysicalQuantityType.Resistivity => "Ω⋅m",
            PhysicalQuantityType.Conductivity => "S/m",
            _ => ""
        };
    }

    public static string GetExponentSymbol(this int exponent)
    {
        return exponent switch
        {
            1 => "",
            2 => "²",
            3 => "³",
            4 => "⁴",
            _ => $"^{exponent}"
        };
    }
}