# Using PhysicalQuantity Instead of Plain Double Values

## The Nightmare Scenario: Plain Doubles

### **The Safe Way (Using PhysicalQuantity)**

```csharp
public class MainViewModel
{
    public PhysicalQuantity InputVoltage { get; set; }
    public PhysicalQuantity InputResistance { get; set; }
    public PhysicalQuantity ResultCurrent => InputVoltage / InputResistance; // Safe!
}
```

### **The Dangerous Way (Plain Doubles)**

```csharp
public class MainViewModel
{
    public double InputVoltageInVolts { get; set; }
    public double InputResistanceInOhms { get; set; }
    public double ResultCurrentInAmps => InputVoltageInVolts / InputResistanceInOhms; // Pray it's right!
}
```

## Why This Is a Disaster Waiting to Happen

### **1️ Dimensional Disaster – Silent Wrong Results**

```csharp
// Doubles: compiles fine, but units make no sense
double voltage = 12.0; // Volts? Millivolts? Who knows!
double time = 5.0;     // Seconds? Hours?
double nonsense = voltage + time; // 17.0 what?? Volt-seconds??
```

```csharp
// PhysicalQuantity: catches it at compile time
var voltage = new PhysicalQuantity(12.0, PhysicalQuantityType.Voltage);
var time = new PhysicalQuantity(5.0, PhysicalQuantityType.Time);
var error = voltage + time; // COMPILE ERROR! Can't add voltage and time
```

### **2️ Unit Hell – Endless Manual Conversion**

```csharp
// Plain double approach – you do all the conversions
public class ElectricalCalculator
{
    public double VoltageInVolts { get; set; }
    public double ResistanceInKiloOhms { get; set; }
    public double CurrentInMilliAmps { get; set; }

    public void Calculate()
    {
        double resistanceInOhms = ResistanceInKiloOhms * 1000.0;
        double currentInAmps = VoltageInVolts / resistanceInOhms;
        CurrentInMilliAmps = currentInAmps * 1000.0;

        // Did you convert everything right? Hope so!
    }
}
```

```csharp
// PhysicalQuantity: conversions handled for you
public void Calculate()
{
    ResultCurrent = InputVoltage / InputResistance; // Units handled automatically
}
```

### **3️ The Copy-Paste Trap**

```csharp
// Doubles: easy to copy-paste wrong units
public class PowerCalculations
{
    public double CalculatePowerLoss()
    {
        double voltage = GetVoltage(); // millivolts
        double current = GetCurrent(); // amperes
        return voltage * current; // WRONG! Units mixed up
    }

    public double CalculateEnergy()
    {
        double power = CalculatePowerLoss(); // Garbage in
        double time = GetTimeHours();
        return power * time; // Garbage out
    }
}

// Silent 1,000,000x error possible
```

### **4️ The Documentation Nightmare**

```csharp
// Doubles: you need endless comments to clarify units
public class EvilDoubleVersion
{
    public double Voltage { get; set; }      // Units: ???
    public double VoltageV { get; set; }     // Units: V
    public double VoltageInVolts { get; set; } // Units: V
    public double VoltageMilliV { get; set; }  // Units: mV
    public double VoltageKV { get; set; }      // Units: kV

    // Which one is right? Who keeps them synced?
}
```

### **5️ Runtime Safety Through Dimensional Analysis**

```csharp
// Doubles: mistakes caught at runtime, maybe never
public double CalculateResonantFrequency(double inductance, double capacitance)
{
    return 1.0 / (2 * Math.PI * Math.Sqrt(inductance * capacitance));
}

// Wrong input - silently produces garbage:
var frequency = CalculateResonantFrequency(
    GetResistance(), // Should be inductance, but compiler doesn't know!
    GetCapacitance()
);
// Result: nonsensical value, no error, just wrong physics
```

```csharp
// PhysicalQuantity: dimensional analysis catches errors at runtime
public PhysicalQuantity CalculateResonantFrequency(PhysicalQuantity inductance, PhysicalQuantity capacitance)
{
    return 1.0 / (2 * Math.PI * (inductance * capacitance).SquareRoot());
}

// Wrong input - throws meaningful exception:
var resistance = new PhysicalQuantity(10.0, PhysicalQuantityType.Resistance);
var capacitance = new PhysicalQuantity(100e-6, PhysicalQuantityType.Capacitance);

try 
{
    var frequency = CalculateResonantFrequency(resistance, capacitance);
}
catch (InvalidOperationException ex)
{
    // "Cannot multiply Resistance and Capacitance: dimensional mismatch"
    // Clear error message explaining the physics violation
}
```

**Key Difference**: 
- **Doubles**: Wrong units produce wrong results silently
- **PhysicalQuantity**: Wrong units throw descriptive physics-aware exceptions

### **6️ UI Binding Catastrophe**

```xml
<!-- Doubles: same value in 3 units = 3 properties -->
<TextBox Text="{Binding VoltageInVolts}"/>
<TextBox Text="{Binding VoltageInMilliVolts}"/>
<TextBox Text="{Binding VoltageInKiloVolts}"/>
```

```xml
<!-- PhysicalQuantity: one value, multiple displays -->
<TextBox>
    <behaviors:PhysicalQuantityBehavior
        Quantity="{Binding Voltage}"
        DisplayUnit="Base"/>
</TextBox>
<TextBox>
    <behaviors:PhysicalQuantityBehavior
        Quantity="{Binding Voltage}"
        DisplayUnit="Milli"/>
</TextBox>
```

### **7️ Real Engineering Disaster**

```csharp
public class MotorController
{
    public double CalculateMotorCurrent(double supplyVoltage, double motorResistance)
    {
        return supplyVoltage / motorResistance;
    }
}

// Wrong resistance entered!
double voltage = 480; // V
double resistance = 2.5; // Ω instead of 2.5mΩ

double current = motorControl.CalculateMotorCurrent(voltage, resistance);
// Expected: ~192A, calculated: 192,000A!
// Consequence: catastrophic failure!
```

## Summary

| Aspect          | Plain Doubles          | PhysicalQuantity           |
| --------------- | ---------------------- | -------------------------- |
| Safety          | Silent wrong results   | Runtime dimensional checks |
| Conversions     | Manual, error-prone    | Automatic                  |
| Readability     | Needs comments         | Self-documenting           |
| Maintainability | Hard                   | Easy                       |
| Error Detection | Often never            | Immediate with clear messages |
| Real-World Cost | High risk              | Reliable                   |

**PhysicalQuantity** = Runtime-safe, self-documenting, automatic conversions, physics-aware.

**Plain doubles** = Silent failures, manual conversions, needs extensive documentation, physics-blind.


