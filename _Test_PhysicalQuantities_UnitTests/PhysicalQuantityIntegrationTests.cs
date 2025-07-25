using FluentAssertions;
using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Physics;

namespace _Test_PhysicalQuantities_UnitTests;

/// <summary>
/// Integration tests that verify PhysicalQuantity operations work correctly with 
/// the dimensional analysis system, testing real physics calculations
/// </summary>
[TestFixture]
public class PhysicalQuantityIntegrationTests
{
    #region Electrical Engineering Tests

    [Test]
    public void OhmsLaw_VoltageDividedByResistance_ShouldProduceCurrent()
    {
        // Arrange
        var voltage = new PhysicalQuantity(12.0, PhysicalQuantityType.Voltage);  // 12V
        var resistance = new PhysicalQuantity(4.0, PhysicalQuantityType.Resistance);  // 4Ω

        // Act
        var current = voltage / resistance;

        // Assert
        current.Type.Should().Be(PhysicalQuantityType.Current);
        current.Value.Should().BeApproximately(3.0, 1e-10); // 12V / 4Ω = 3A
        current.Exponent.Should().Be(1);
    }

    [Test]
    public void PowerCalculation_VoltageTimesCurrent_ShouldProducePower()
    {
        // Arrange - P = V × I
        var voltage = new PhysicalQuantity(120.0, PhysicalQuantityType.Voltage);  // 120V
        var current = new PhysicalQuantity(5.0, PhysicalQuantityType.Current);   // 5A

        // Act
        var power = voltage * current;

        // Assert
        power.Type.Should().Be(PhysicalQuantityType.Power);
        power.Value.Should().BeApproximately(600.0, 1e-10); // 120V × 5A = 600W
    }

    [Test]
    public void PowerCalculation_VoltageSquaredDividedByResistance_ShouldProducePower()
    {
        // Arrange - P = V² / R
        var voltage = new PhysicalQuantity(10.0, PhysicalQuantityType.Voltage);  // 10V
        var resistance = new PhysicalQuantity(2.0, PhysicalQuantityType.Resistance);  // 2Ω

        // Act
        var voltageSquared = voltage * voltage; // Should create V²
        var power = voltageSquared / resistance;

        // Assert
        power.Type.Should().Be(PhysicalQuantityType.Power);
        power.Value.Should().BeApproximately(50.0, 1e-10); // 10² / 2 = 100 / 2 = 50W
    }

    [Test]
    public void EnergyCalculation_PowerTimesTime_ShouldProduceEnergy()
    {
        // Arrange - E = P × t
        var power = new PhysicalQuantity(1000.0, PhysicalQuantityType.Power);  // 1000W
        var time = new PhysicalQuantity(3600.0, PhysicalQuantityType.Time);    // 1 hour = 3600s

        // Act
        var energy = power * time;

        // Assert
        energy.Type.Should().Be(PhysicalQuantityType.Energy);
        power.Value.Should().BeApproximately(1000.0, 1e-10);
        time.Value.Should().BeApproximately(3600.0, 1e-10);
        energy.Value.Should().BeApproximately(3600000.0, 1e-10); // 1000W × 3600s = 3.6MJ
    }

    [Test]
    public void CapacitiveReactance_InverseTwoTimesFrequencyTimesCapacitance_ShouldProduceReactance()
    {
        // Arrange - Xc = 1 / (2π × f × C), but we will test the dimensional analysis part
        var frequency = new PhysicalQuantity(60.0, PhysicalQuantityType.Frequency);      // 60 Hz
        var capacitance = new PhysicalQuantity(100e-6, PhysicalQuantityType.Capacitance); // 100μF

        // Act - Test that frequency × capacitance gives the right dimension to produce reactance
        var frequencyCapacitanceProduct = frequency * capacitance;

        // The dimensional analysis should show that 1/(Hz × F) = Ω
        // Verify the types are set up correctly
        frequencyCapacitanceProduct.Type.Should().Be(PhysicalQuantityType.Conductance); // Hz × F should give conductance (1/Ω)
    }

    #endregion

    #region Mechanical Engineering Tests

    /*[Test]
    public void NewtonsSecondLaw_MassTimesAcceleration_ShouldProduceForce()
    {
        // Arrange - F = m × a
        var mass = new PhysicalQuantity(10.0, PhysicalQuantityType.Mass);              // 10 kg
        var acceleration = new PhysicalQuantity(9.81, PhysicalQuantityType.Acceleration); // 9.81 m/s²

        // Act
        var force = mass * acceleration;

        // Assert
        force.Type.Should().Be(PhysicalQuantityType.Force);
        force.Value.Should().BeApproximately(98.1, 1e-10); // 10 kg × 9.81 m/s² = 98.1 N
    }

    [Test]
    public void KineticEnergy_HalfMassTimesVelocitySquared_ShouldProduceEnergy()
    {
        // Arrange - KE = ½mv² (we'll test the dimensional part: m × v²)
        var mass = new PhysicalQuantity(5.0, PhysicalQuantityType.Mass);        // 5 kg
        var velocity = new PhysicalQuantity(10.0, PhysicalQuantityType.Velocity); // 10 m/s

        // Act
        var velocitySquared = velocity * velocity; // v²
        var kineticEnergyDimensional = mass * velocitySquared; // m × v²

        // Assert
        kineticEnergyDimensional.Type.Should().Be(PhysicalQuantityType.Energy);
        // Value would be 5 × 100 = 500, but real kinetic energy is ½ × 500 = 250 J
        kineticEnergyDimensional.Value.Should().BeApproximately(500.0, 1e-10);
    }

    [Test]
    public void Pressure_ForceDividedByArea_ShouldProducePressure()
    {
        // Arrange - P = F / A
        var force = new PhysicalQuantity(1000.0, PhysicalQuantityType.Force);  // 1000 N
        var area = new PhysicalQuantity(0.1, PhysicalQuantityType.Area);       // 0.1 m²

        // Act
        var pressure = force / area;

        // Assert
        pressure.Type.Should().Be(PhysicalQuantityType.Pressure);
        pressure.Value.Should().BeApproximately(10000.0, 1e-10); // 1000 N / 0.1 m² = 10,000 Pa
    }

    [Test]
    public void Density_MassDividedByVolume_ShouldProduceDensity()
    {
        // Arrange - ρ = m / V
        var mass = new PhysicalQuantity(800.0, PhysicalQuantityType.Mass);     // 800 kg
        var volume = new PhysicalQuantity(1.0, PhysicalQuantityType.Volume);   // 1 m³

        // Act
        var density = mass / volume;

        // Assert
        density.Type.Should().Be(PhysicalQuantityType.Density);
        density.Value.Should().BeApproximately(800.0, 1e-10); // 800 kg/m³
    }*/

    #endregion

    #region Thermodynamics Tests

    /* [Test]
     public void HeatFlux_PowerDividedByArea_ShouldProduceHeatFlux()
     {
         // Arrange - Heat flux = Power / Area
         var power = new PhysicalQuantity(2000.0, PhysicalQuantityType.Power);  // 2000 W
         var area = new PhysicalQuantity(4.0, PhysicalQuantityType.Area);       // 4 m²

         // Act
         var heatFlux = power / area;

         // Assert
         heatFlux.Type.Should().Be(PhysicalQuantityType.HeatFlux);
         heatFlux.Value.Should().BeApproximately(500.0, 1e-10); // 2000 W / 4 m² = 500 W/m²
     }*/

    #endregion

    #region Complex Multi-Step Calculations

    [Test]
    public void ComplexElectricalCircuit_MultipleOperations_ShouldProduceCorrectResults()
    {
        // Arrange - Complex calculation: P = I²R, where I = V/R
        var voltage = new PhysicalQuantity(24.0, PhysicalQuantityType.Voltage);      // 24V
        var resistance = new PhysicalQuantity(6.0, PhysicalQuantityType.Resistance); // 6Ω

        // Act
        var current = voltage / resistance;                    // I = V/R = 4A
        var currentSquared = current * current;                // I² = 16 A²
        var powerLoss = currentSquared * resistance;           // P = I²R

        // Assert
        current.Type.Should().Be(PhysicalQuantityType.Current);
        current.Value.Should().BeApproximately(4.0, 1e-10);

        powerLoss.Type.Should().Be(PhysicalQuantityType.Power);
        powerLoss.Value.Should().BeApproximately(96.0, 1e-10); // I²R = 16 × 6 = 96W
    }

    [Test]
    public void MechanicalWorkAndEnergy_ForceTimesDistance_ShouldProduceEnergy()
    {
        // Arrange - Work = Force × Distance
        var force = new PhysicalQuantity(50.0, PhysicalQuantityType.Force);    // 50 N
        var distance = new PhysicalQuantity(10.0, PhysicalQuantityType.Length); // 10 m

        // Act
        var work = force * distance;

        // Assert
        work.Type.Should().Be(PhysicalQuantityType.Energy);
        work.Value.Should().BeApproximately(500.0, 1e-10); // 50 N × 10 m = 500 J
    }

    /*[Test]
    public void ElectricalAndMechanicalPowerEquivalence_ShouldBothProducePower()
    {
        // Arrange
        var voltage = new PhysicalQuantity(120.0, PhysicalQuantityType.Voltage);
        var current = new PhysicalQuantity(10.0, PhysicalQuantityType.Current);

        var force = new PhysicalQuantity(100.0, PhysicalQuantityType.Force);
        var velocity = new PhysicalQuantity(12.0, PhysicalQuantityType.Velocity);

        // Act
        var electricalPower = voltage * current;    // P = VI
        var mechanicalPower = force * velocity;     // P = Fv

        // Assert - Both should produce Power type
        electricalPower.Type.Should().Be(PhysicalQuantityType.Power);
        mechanicalPower.Type.Should().Be(PhysicalQuantityType.Power);

        electricalPower.Value.Should().BeApproximately(1200.0, 1e-10); // 120V × 10A = 1200W
        mechanicalPower.Value.Should().BeApproximately(1200.0, 1e-10); // 100N × 12m/s = 1200W
    }*/

    #endregion

    #region Unit Prefix Integration Tests

    [Test]
    public void UnitPrefixes_InCalculations_ShouldMaintainDimensionalCorrectness()
    {
        // Arrange - Test with different unit prefixes
        var voltage = new PhysicalQuantity(5.0, PhysicalQuantityType.Voltage, 1, UnitPrefix.Kilo);   // 5 kV
        var resistance = new PhysicalQuantity(2.5, PhysicalQuantityType.Resistance, 1, UnitPrefix.Kilo); // 2.5 kΩ

        // Act
        var current = voltage / resistance;

        // Assert
        current.Type.Should().Be(PhysicalQuantityType.Current);
        current.Value.Should().BeApproximately(2.0, 1e-10); // 5000V / 2500Ω = 2A
    }

    [Test]
    public void MixedUnitPrefixes_ShouldProduceCorrectResults()
    {
        // Arrange
        var voltage = new PhysicalQuantity(12.0, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);    // 12 V
        var current = new PhysicalQuantity(500.0, PhysicalQuantityType.Current, 1, UnitPrefix.Milli); // 500 mA

        // Act
        var power = voltage * current;

        // Assert
        power.Type.Should().Be(PhysicalQuantityType.Power);
        power.Value.Should().BeApproximately(6.0, 1e-10); // 12V × 0.5A = 6W
    }

    #endregion

    #region Edge Cases and Error Conditions

    [Test]
    public void IncompatibleUnits_Addition_ShouldThrowException()
    {
        // Arrange
        var voltage = new PhysicalQuantity(12.0, PhysicalQuantityType.Voltage);
        var current = new PhysicalQuantity(2.0, PhysicalQuantityType.Current);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => { var result = voltage + current; });
    }

    [Test]
    public void UnknownDimensionalCombination_ShouldThrowException()
    {
        // Arrange - Create a combination that doesn't exist in the physics definitions
        var temperature = new PhysicalQuantity(25.0, PhysicalQuantityType.Temperature);
        var charge = new PhysicalQuantity(1.0, PhysicalQuantityType.Charge);

        // Act & Assert - Temperature × Charge should throw an exception if not defined
        Assert.Throws<InvalidOperationException>(() => { var result = temperature * charge; });
    }

    #endregion
}