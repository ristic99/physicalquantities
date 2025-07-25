using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Physics;
using FluentAssertions;

namespace _Test_PhysicalQuantities_UnitTests
{
    [TestFixture]
    public class DimensionalAnalysisEngineUnitTests
    {
        #region Physics Discovery Tests

        [Test]
        public void FindAllWaysToCreate_Power_ShouldReturnExpectedCombinations()
        {
            // Act
            var powerCreationMethods = DimensionalAnalysisEngine.FindAllWaysToCreate(PhysicalQuantityType.Power);

            // Assert
            var methods = powerCreationMethods.ToList();
            methods.Should().NotBeEmpty();

            // Should include common ways to create power
            methods.Should().Contain(s => s.Contains("Voltage") && s.Contains("Current") && s.Contains("×"));
        }

        [Test]
        public void FindAllWaysToCreate_Current_ShouldReturnExpectedCombinations()
        {
            // Act
            var currentCreationMethods = DimensionalAnalysisEngine.FindAllWaysToCreate(PhysicalQuantityType.Current);

            // Assert
            var methods = currentCreationMethods.ToList();
            methods.Should().NotBeEmpty();

            // Should include Ohm's law: V ÷ R = I
            methods.Should().Contain(s => s.Contains("Voltage") && s.Contains("Resistance") && s.Contains("÷"));
        }

        [Test]
        public void FindAllWaysToCreate_Energy_ShouldReturnMultipleCombinations()
        {
            // Act
            var energyCreationMethods = DimensionalAnalysisEngine.FindAllWaysToCreate(PhysicalQuantityType.Energy);

            // Assert
            var methods = energyCreationMethods.ToList();
            methods.Should().NotBeEmpty();

            // Should include P×t, F×d, etc.
            methods.Should().Contain(s => s.Contains("Power") && s.Contains("Time") && s.Contains("×"));
            methods.Should().Contain(s => s.Contains("Force") && s.Contains("Length") && s.Contains("×"));
        }

        #endregion
    }
}
