using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Utils;

namespace PhysicalQuantities.Core.Physics;

/// <summary>
/// 
/// </summary>
public static class DimensionalAnalysisEngine
{
    private static readonly FrozenBiDictionary<PhysicalQuantityType, DimensionalFormula> QuantityDimension;

    static DimensionalAnalysisEngine()
    {
        QuantityDimension = new FrozenBiDictionary<PhysicalQuantityType, DimensionalFormula>(
            PhysicsDefinitions.QuantityDimensions
        );
    }

    public static PhysicalQuantityType MultiplyQuantities(PhysicalQuantityType typeA, int exponentA, PhysicalQuantityType typeB, int exponentB)
    {
        var dimensionA = GetDimensionWithExponent(typeA, exponentA);
        var dimensionB = GetDimensionWithExponent(typeB, exponentB);
        var resultDimension = dimensionA * dimensionB;
        return FindQuantityType(resultDimension);
    }

    public static PhysicalQuantityType DivideQuantities(PhysicalQuantityType typeA, int exponentA, PhysicalQuantityType typeB, int exponentB)
    {
        var dimensionA = GetDimensionWithExponent(typeA, exponentA);
        var dimensionB = GetDimensionWithExponent(typeB, exponentB);
        var resultDimension = dimensionA / dimensionB;
        return FindQuantityType(resultDimension);
    }

    private static DimensionalFormula GetDimensionWithExponent(PhysicalQuantityType type, int exponent) => QuantityDimension[type].RaiseToPower(exponent);

    private static PhysicalQuantityType FindQuantityType(DimensionalFormula dimension)
    {
        if (QuantityDimension.TryGetByValue(dimension, out var resultType))
            return resultType;

        throw new InvalidOperationException($"Unknown dimensional combination: {dimension.ToString()}");
    }

    // Useful debugging method
    public static IEnumerable<string> FindAllWaysToCreate(PhysicalQuantityType target)
    {
        var targetDimension = QuantityDimension.Forward[target];
        var results = new HashSet<string>();

        var map = QuantityDimension.Forward;

        foreach (var (typeA, dimA) in map)
        {
            var dimASq = dimA.RaiseToPower(2);

            foreach (var (typeB, dimB) in map)
            {
                var dimBSq = dimB.RaiseToPower(2);

                // Basic
                if ((dimA * dimB).Equals(targetDimension))
                    results.Add($"{typeA} × {typeB} = {target}");

                if ((dimA / dimB).Equals(targetDimension))
                    results.Add($"{typeA} ÷ {typeB} = {target}");

                // Squared A
                if ((dimASq * dimB).Equals(targetDimension))
                    results.Add($"{typeA}² × {typeB} = {target}");

                if ((dimASq / dimB).Equals(targetDimension))
                    results.Add($"{typeA}² ÷ {typeB} = {target}");

                // Squared B (in denominator)
                if ((dimA / dimBSq).Equals(targetDimension))
                    results.Add($"{typeA} ÷ {typeB}² = {target}");
            }
        }

        return results;
    }
}