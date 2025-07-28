using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Physics;

namespace PhysicalQuantities.Models;

/// <summary>
/// Simple electrical circuit model for Ohm's law calculations: V = I × R
/// </summary>
public class ElectricalCircuit
{
    public PhysicalQuantity Voltage { get; set; }
    public PhysicalQuantity Resistance { get; set; }

    public PhysicalQuantity Current => Voltage / Resistance;

    public ElectricalCircuit()
    {
        Voltage = new PhysicalQuantity(0, PhysicalQuantityType.Voltage);
        Resistance = new PhysicalQuantity(1, PhysicalQuantityType.Resistance); // Avoid division by zero
    }

    public ElectricalCircuit(PhysicalQuantity voltage, PhysicalQuantity resistance)
    {
        Voltage = voltage;
        Resistance = resistance;
    }
}