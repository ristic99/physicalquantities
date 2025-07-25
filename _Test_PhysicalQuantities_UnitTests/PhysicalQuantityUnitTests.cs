using FluentAssertions;
using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Physics;

namespace _Test_PhysicalQuantities_UnitTests;

/// <summary>
/// Pure unit tests for PhysicalQuantity struct - testing in isolation without dependencies
/// </summary>
[TestFixture]
public class PhysicalQuantityUnitTests
{
    [Test]
    public void Constructor_ShouldCreatePhysicalQuantity_WithCorrectBaseValue()
    {
        // Arrange & Act
        var voltage = new PhysicalQuantity(12.0, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);

        // Assert
        voltage.Value.Should().Be(12.0);
        voltage.Type.Should().Be(PhysicalQuantityType.Voltage);
        voltage.Exponent.Should().Be(1);
    }

    [Test]
    public void Constructor_WithPrefix_ShouldConvertToBaseUnits()
    {
        // Arrange & Act
        var voltage = new PhysicalQuantity(1000, PhysicalQuantityType.Voltage, 1, UnitPrefix.Milli);

        // Assert - should convert 1000mV to 1V internally
        voltage.Value.Should().Be(1.0);
        voltage.Type.Should().Be(PhysicalQuantityType.Voltage);
        voltage.Exponent.Should().Be(1);
    }

    [Test]
    public void UnitConversion_VoltsToMillivolts_ShouldBeEqual()
    {
        // Arrange
        var volt = new PhysicalQuantity(1, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var voltInMillivolts = new PhysicalQuantity(1000, PhysicalQuantityType.Voltage, 1, UnitPrefix.Milli);

        // Act & Assert
        volt.Should().Be(voltInMillivolts);
        (volt == voltInMillivolts).Should().BeTrue();
        (volt != voltInMillivolts).Should().BeFalse();
    }

    [TestCase(1, UnitPrefix.Base, 1000, UnitPrefix.Milli)]
    [TestCase(1, UnitPrefix.Base, 1000000, UnitPrefix.Micro)]
    [TestCase(1, UnitPrefix.Base, 1000000000, UnitPrefix.Nano)]
    [TestCase(1, UnitPrefix.Kilo, 1000, UnitPrefix.Base)]
    [TestCase(1, UnitPrefix.Mega, 1000, UnitPrefix.Kilo)]
    [TestCase(1, UnitPrefix.Giga, 1000, UnitPrefix.Mega)]
    public void UnitConversion_DifferentPrefixes_ShouldBeEqual(
        double value1, UnitPrefix prefix1,
        double value2, UnitPrefix prefix2)
    {
        // Arrange
        var quantity1 = new PhysicalQuantity(value1, PhysicalQuantityType.Voltage, 1, prefix1);
        var quantity2 = new PhysicalQuantity(value2, PhysicalQuantityType.Voltage, 1, prefix2);

        // Act & Assert
        quantity1.Should().Be(quantity2);
    }

    [Test]
    public void GetValueIn_ShouldReturnCorrectConvertedValue()
    {
        // Arrange
        var voltage = new PhysicalQuantity(5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);

        // Act & Assert
        voltage.GetValueIn(UnitPrefix.Base).Should().Be(5.0);
        voltage.GetValueIn(UnitPrefix.Milli).Should().Be(5000.0);
        voltage.GetValueIn(UnitPrefix.Micro).Should().Be(5000000.0);
        voltage.GetValueIn(UnitPrefix.Kilo).Should().Be(0.005);
    }

    [Test]
    public void Equality_DifferentQuantityTypes_ShouldNotBeEqual()
    {
        // Arrange
        var voltage = new PhysicalQuantity(1, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var current = new PhysicalQuantity(1, PhysicalQuantityType.Current, 1, UnitPrefix.Base);

        // Act & Assert
        voltage.Should().NotBe(current);
        (voltage == current).Should().BeFalse();
        (voltage != current).Should().BeTrue();
    }

    [Test]
    public void Equality_DifferentExponents_ShouldNotBeEqual()
    {
        // Arrange
        var voltage = new PhysicalQuantity(1, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var voltageSquared = new PhysicalQuantity(1, PhysicalQuantityType.Voltage, 2, UnitPrefix.Base);

        // Act & Assert
        voltage.Should().NotBe(voltageSquared);
    }

    [Test]
    public void Equality_SlightlyDifferentValues_WithinTolerance_ShouldBeEqual()
    {
        // Arrange (values within 1e-12 tolerance)
        var value1 = new PhysicalQuantity(1.0, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var value2 = new PhysicalQuantity(1.0 + 1e-13, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);

        // Act & Assert
        value1.Should().Be(value2);
    }

    [Test]
    public void Equality_DifferentValues_OutsideTolerance_ShouldNotBeEqual()
    {
        // Arrange (values outside 1e-12 tolerance)
        var value1 = new PhysicalQuantity(1.0, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var value2 = new PhysicalQuantity(1.0 + 1e-11, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);

        // Act & Assert
        value1.Should().NotBe(value2);
    }

    [Test]
    public void ToString_BaseUnits_ShouldReturnFormattedString()
    {
        // Arrange
        var voltage = new PhysicalQuantity(12.5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);

        // Act
        var result = voltage.ToString();

        // Assert
        result.Should().Be("12.5 V");
    }

    [Test]
    public void ToString_WithPrefix_ShouldReturnFormattedStringWithPrefix()
    {
        // Arrange
        var voltage = new PhysicalQuantity(12.5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);

        // Act
        var result = voltage.ToString(UnitPrefix.Milli);

        // Assert
        result.Should().Be("12500 mV");
    }

    [TestCase(2.5, PhysicalQuantityType.Resistance, 1, UnitPrefix.Kilo, UnitPrefix.Kilo, "2.5 kΩ")]
    [TestCase(150, PhysicalQuantityType.Current, 1, UnitPrefix.Milli, UnitPrefix.Milli, "150 mA")]
    [TestCase(47, PhysicalQuantityType.Capacitance, 1, UnitPrefix.Micro, UnitPrefix.Micro, "47 μF")]
    [TestCase(100, PhysicalQuantityType.Power, 1, UnitPrefix.Base, UnitPrefix.Base, "100 W")]
    // Test cases with different conversion prefixes
    [TestCase(1, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base, UnitPrefix.Milli, "1000 mV")]
    [TestCase(5000, PhysicalQuantityType.Voltage, 1, UnitPrefix.Milli, UnitPrefix.Base, "5 V")]
    [TestCase(2.5, PhysicalQuantityType.Resistance, 1, UnitPrefix.Kilo, UnitPrefix.Base, "2500 Ω")]
    [TestCase(1000, PhysicalQuantityType.Resistance, 1, UnitPrefix.Base, UnitPrefix.Kilo, "1 kΩ")]
    [TestCase(50, PhysicalQuantityType.Current, 1, UnitPrefix.Milli, UnitPrefix.Micro, "50000 μA")]
    [TestCase(100000, PhysicalQuantityType.Capacitance, 1, UnitPrefix.Micro, UnitPrefix.Milli, "100 mF")]
    [TestCase(0.5, PhysicalQuantityType.Power, 1, UnitPrefix.Base, UnitPrefix.Milli, "500 mW")]
    [TestCase(2000, PhysicalQuantityType.Power, 1, UnitPrefix.Milli, UnitPrefix.Base, "2 W")]
    public void ToString_DifferentQuantityTypes_ShouldReturnCorrectFormat(
        double value, PhysicalQuantityType type, int exponent, UnitPrefix prefix, UnitPrefix convertToPrefix, string expected)
    {
        // Arrange
        var quantity = new PhysicalQuantity(value, type, exponent, prefix);

        // Act
        var result = quantity.ToString(convertToPrefix);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void Addition_CompatibleQuantities_ShouldWork()
    {
        // Arrange
        var voltage1 = new PhysicalQuantity(5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var voltage2 = new PhysicalQuantity(3000, PhysicalQuantityType.Voltage, 1, UnitPrefix.Milli);

        // Act
        var result = voltage1 + voltage2;

        // Assert
        result.Value.Should().Be(8.0); // 5V + 3V = 8V
        result.Type.Should().Be(PhysicalQuantityType.Voltage);
        result.Exponent.Should().Be(1);
    }

    [Test]
    public void Addition_IncompatibleQuantities_ShouldThrowException()
    {
        // Arrange
        var voltage = new PhysicalQuantity(5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var current = new PhysicalQuantity(2, PhysicalQuantityType.Current, 1, UnitPrefix.Base);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => { var result = voltage + current; });
    }

    [Test]
    public void Subtraction_CompatibleQuantities_ShouldWork()
    {
        // Arrange
        var voltage1 = new PhysicalQuantity(5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var voltage2 = new PhysicalQuantity(2000, PhysicalQuantityType.Voltage, 1, UnitPrefix.Milli);

        // Act
        var result = voltage1 - voltage2;

        // Assert
        result.Value.Should().Be(3.0); // 5V - 2V = 3V
        result.Type.Should().Be(PhysicalQuantityType.Voltage);
    }

    [Test]
    public void Multiplication_WithScalar_ShouldWork()
    {
        // Arrange
        var voltage = new PhysicalQuantity(5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);

        // Act
        var result = voltage * 2.0;

        // Assert
        result.Value.Should().Be(10.0);
        result.Type.Should().Be(PhysicalQuantityType.Voltage);
        result.Exponent.Should().Be(1);
    }

    [Test]
    public void Division_WithScalar_ShouldWork()
    {
        // Arrange
        var voltage = new PhysicalQuantity(10, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);

        // Act
        var result = voltage / 2.0;

        // Assert
        result.Value.Should().Be(5.0);
        result.Type.Should().Be(PhysicalQuantityType.Voltage);
        result.Exponent.Should().Be(1);
    }

    [Test]
    public void IsCompatibleForAddition_SameTypeAndExponent_ShouldReturnTrue()
    {
        // Arrange
        var voltage1 = new PhysicalQuantity(5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var voltage2 = new PhysicalQuantity(3000, PhysicalQuantityType.Voltage, 1, UnitPrefix.Milli);

        // Act & Assert
        voltage1.IsCompatibleForAddition(voltage2).Should().BeTrue();
    }

    [Test]
    public void IsCompatibleForAddition_DifferentTypes_ShouldReturnFalse()
    {
        // Arrange
        var voltage = new PhysicalQuantity(5, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
        var current = new PhysicalQuantity(2, PhysicalQuantityType.Current, 1, UnitPrefix.Base);

        // Act & Assert
        voltage.IsCompatibleForAddition(current).Should().BeFalse();
    }
}