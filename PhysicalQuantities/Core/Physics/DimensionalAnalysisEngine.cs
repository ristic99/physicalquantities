using PhysicalQuantities.Core.Enums;
using System.Collections.Frozen;

namespace PhysicalQuantities.Core.Physics;

/// <summary>
/// 
/// </summary>
public static class DimensionalAnalysisEngine
{
    private static readonly FrozenDictionary<PhysicalQuantityType, DimensionalFormula> QuantityToDimension;
    private static readonly FrozenDictionary<DimensionalFormula, PhysicalQuantityType> DimensionToQuantity;

    static DimensionalAnalysisEngine()
    {
        QuantityToDimension = PhysicsDefinitions.QuantityDimensions.ToFrozenDictionary();
        DimensionToQuantity = PhysicsDefinitions.QuantityDimensions.ToDictionary(kvp => kvp.Value, kvp => kvp.Key).ToFrozenDictionary();
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

    private static DimensionalFormula GetDimensionWithExponent(PhysicalQuantityType type, int exponent) => QuantityToDimension[type].RaiseToPower(exponent);

    private static PhysicalQuantityType FindQuantityType(DimensionalFormula dimension)
    {
        if (DimensionToQuantity.TryGetValue(dimension, out var resultType))
            return resultType;

        throw new InvalidOperationException($"Unknown dimensional combination: {dimension.ToString()}");
    }

    // Useful debugging method
    public static IEnumerable<string> FindAllWaysToCreate(PhysicalQuantityType target)
    {
        var targetDimension = QuantityToDimension[target];
        var results = new List<string>();

        foreach (var (typeA, dimA) in QuantityToDimension)
        {
            foreach (var (typeB, dimB) in QuantityToDimension)
            {
                // Basic operations
                if ((dimA * dimB).Equals(targetDimension))
                    results.Add($"{typeA} × {typeB} = {target}");

                if ((dimA / dimB).Equals(targetDimension))
                    results.Add($"{typeA} ÷ {typeB} = {target}");

                // Squared operations
                var dimASquared = dimA.RaiseToPower(2);
                if ((dimASquared * dimB).Equals(targetDimension))
                    results.Add($"{typeA}² × {typeB} = {target}");

                if ((dimASquared / dimB).Equals(targetDimension))
                    results.Add($"{typeA}² ÷ {typeB} = {target}");

                if ((dimA / dimB.RaiseToPower(2)).Equals(targetDimension))
                    results.Add($"{typeA} ÷ {typeB}² = {target}");
            }
        }

        return results.Distinct();
    }
}