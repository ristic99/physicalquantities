
using PhysicalQuantities.Core.Enums;

namespace PhysicalQuantities.Core.Physics;

/// <summary>
/// Dimensional analysis engine with support for quantity nature and operation types
/// </summary>
public static class DimensionalAnalysisEngine
{
    public static PhysicalQuantityType MultiplyQuantities(
        PhysicalQuantityType typeA, QuantityNature natureA, int exponentA,
        PhysicalQuantityType typeB, QuantityNature natureB, int exponentB,
        OperationType operationType)
    {
        var dimensionA = GetDimensionWithExponent(typeA, exponentA);
        var dimensionB = GetDimensionWithExponent(typeB, exponentB);
        var resultDimension = dimensionA * dimensionB;

        var resultNature = DetermineResultNature(natureA, natureB, operationType);

        return PhysicsDefinitions.FindQuantityType(resultDimension, resultNature, operationType);
    }

    public static PhysicalQuantityType DivideQuantities(
        PhysicalQuantityType typeA, QuantityNature natureA, int exponentA,
        PhysicalQuantityType typeB, QuantityNature natureB, int exponentB,
        OperationType operationType)
    {
        var dimensionA = GetDimensionWithExponent(typeA, exponentA);
        var dimensionB = GetDimensionWithExponent(typeB, exponentB);
        var resultDimension = dimensionA / dimensionB;

        // Za deljenje, priroda je uglavnom skalarna osim u posebnim slučajevima
        var resultNature = DetermineResultNatureForDivision(natureA, natureB);

        return PhysicsDefinitions.FindQuantityType(resultDimension, resultNature, operationType);
    }

    private static DimensionalFormula GetDimensionWithExponent(PhysicalQuantityType type, int exponent)
    {
        var baseDimension = PhysicsDefinitions.GetDimensions(type);
        return baseDimension.RaiseToPower(exponent);
    }

    private static QuantityNature DetermineResultNature(
        QuantityNature a,
        QuantityNature b,
        OperationType operationType)
    {
        return operationType switch
        {
            OperationType.ScalarMultiply => DetermineScalarMultiplyNature(a, b),
            OperationType.DotProduct => QuantityNature.Scalar,
            OperationType.CrossProduct => QuantityNature.Pseudovector,
            OperationType.Direct => QuantityNature.Scalar,
            _ => QuantityNature.Scalar
        };
    }

    private static QuantityNature DetermineScalarMultiplyNature(QuantityNature a, QuantityNature b)
    {
        // Scalar × anything = that thing
        if (a == QuantityNature.Scalar) return b;
        if (b == QuantityNature.Scalar) return a;

        // Vector × Vector = složenije (ali za ScalarMultiply obično ne bi trebalo)
        // Za sada vraćamo Vector kao default
        return QuantityNature.Vector;
    }

    private static QuantityNature DetermineResultNatureForDivision(QuantityNature a, QuantityNature b)
    {
        // Vector / Scalar = Vector
        if (b == QuantityNature.Scalar) return a;

        // Ostalo je uglavnom Scalar
        return QuantityNature.Scalar;
    }

    // Debugging/exploration metoda (može ostati za development)
    public static IEnumerable<string> FindAllWaysToCreate(PhysicalQuantityType target)
    {
        var targetDimension = PhysicsDefinitions.GetDimensions(target);
        var results = new HashSet<string>();

        // Dobij sve tipove iz Registry-ja
        var allTypes = Enum.GetValues<PhysicalQuantityType>()
            .Where(t => t != PhysicalQuantityType.Dimensionless);

        foreach (var typeA in allTypes)
        {
            var dimA = PhysicsDefinitions.GetDimensions(typeA);
            var dimASq = dimA.RaiseToPower(2);

            foreach (var typeB in allTypes)
            {
                var dimB = PhysicsDefinitions.GetDimensions(typeB);
                var dimBSq = dimB.RaiseToPower(2);

                // Basic operations
                if ((dimA * dimB).Equals(targetDimension))
                    results.Add($"{typeA} × {typeB} = {target}");

                if ((dimA / dimB).Equals(targetDimension))
                    results.Add($"{typeA} ÷ {typeB} = {target}");

                // Squared A
                if ((dimASq * dimB).Equals(targetDimension))
                    results.Add($"{typeA}² × {typeB} = {target}");

                if ((dimASq / dimB).Equals(targetDimension))
                    results.Add($"{typeA}² ÷ {typeB} = {target}");

                // Squared B
                if ((dimA / dimBSq).Equals(targetDimension))
                    results.Add($"{typeA} ÷ {typeB}² = {target}");
            }
        }

        return results.OrderBy(s => s);
    }
}