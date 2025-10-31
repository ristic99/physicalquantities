using PhysicalQuantities.Core.Enums;

namespace PhysicalQuantities.Core.Physics;

public readonly struct PhysicalQuantityBase
{
    public PhysicalQuantityBase(double value, DimensionalFormula dimensions, QuantityNature nature, OperationType createdBy)
    {
        Value = value;
        Dimensions = dimensions;
        Nature = nature;
        CreatedBy = createdBy;
    }

    public double Value { get; }
    public DimensionalFormula Dimensions { get; }
    public QuantityNature Nature { get; }
    public OperationType CreatedBy { get; }

    // ============================================
    // Generički operatori
    // ============================================

    /// <summary>
    /// Dot product: this · other → Always returns Scalar
    /// </summary>
    public OperationResult Dot(PhysicalQuantityBase other)
    {
        if (this.Nature != QuantityNature.Vector || other.Nature != QuantityNature.Vector)
            throw new InvalidOperationException("Dot product requires two vectors");

        var resultDimensions = this.Dimensions * other.Dimensions;
        var resultValue = this.Value * other.Value;

        // Dot product UVEK vraća Scalar
        return new OperationResult(
            resultValue,
            resultDimensions,
            QuantityNature.Scalar,  // ← Automatski!
            OperationType.DotProduct
        );
    }

    /// <summary>
    /// Cross product: this × other → Always returns Pseudovector
    /// </summary>
    public OperationResult Cross(PhysicalQuantityBase other)
    {
        if (this.Nature != QuantityNature.Vector || other.Nature != QuantityNature.Vector)
            throw new InvalidOperationException("Cross product requires two vectors");

        var resultDimensions = this.Dimensions * other.Dimensions;
        var resultValue = this.Value * other.Value;

        // Cross product UVEK vraća Pseudovector
        return new OperationResult(
            resultValue,
            resultDimensions,
            QuantityNature.Pseudovector,  // ← Automatski!
            OperationType.CrossProduct
        );
    }

    /// <summary>
    /// Scalar multiplication: this × scalar
    /// Preserves the nature of the vector
    /// </summary>
    public OperationResult ScalarMultiply(double scalar)
    {
        return new OperationResult(
            this.Value * scalar,
            this.Dimensions,
            this.Nature,  // ← Čuva prirodu!
            OperationType.ScalarMultiply
        );
    }

    /// <summary>
    /// Multiply two PhysicalQuantityBase objects
    /// Automatically determines result nature based on input natures
    /// </summary>
    public static OperationResult operator *(PhysicalQuantityBase a, PhysicalQuantityBase b)
    {
        var resultDimensions = a.Dimensions * b.Dimensions;
        var resultValue = a.Value * b.Value;

        // Automatski odredi prirodu i tip operacije
        var (resultNature, operationType) = DetermineMultiplicationResult(a.Nature, b.Nature);

        return new OperationResult(
            resultValue,
            resultDimensions,
            resultNature,
            operationType
        );
    }

    /// <summary>
    /// Divide two PhysicalQuantityBase objects
    /// </summary>
    public static OperationResult operator /(PhysicalQuantityBase a, PhysicalQuantityBase b)
    {
        var resultDimensions = a.Dimensions / b.Dimensions;
        var resultValue = a.Value / b.Value;

        // Automatski odredi prirodu
        var resultNature = DetermineDivisionResult(a.Nature, b.Nature);

        return new OperationResult(
            resultValue,
            resultDimensions,
            resultNature,
            OperationType.ScalarMultiply  // Division is treated as scalar operation
        );
    }

    // ============================================
    // Helper metode za određivanje Nature
    // ============================================

    private static (QuantityNature resultNature, OperationType operationType) DetermineMultiplicationResult(
        QuantityNature a, QuantityNature b)
    {
        return (a, b) switch
        {
            // Scalar × Scalar = Scalar
            (QuantityNature.Scalar, QuantityNature.Scalar) =>
                (QuantityNature.Scalar, OperationType.ScalarMultiply),

            // Scalar × Vector = Vector (skaliranje)
            (QuantityNature.Scalar, QuantityNature.Vector) =>
                (QuantityNature.Vector, OperationType.ScalarMultiply),
            (QuantityNature.Vector, QuantityNature.Scalar) =>
                (QuantityNature.Vector, OperationType.ScalarMultiply),

            // Scalar × Pseudovector = Pseudovector
            (QuantityNature.Scalar, QuantityNature.Pseudovector) =>
                (QuantityNature.Pseudovector, OperationType.ScalarMultiply),
            (QuantityNature.Pseudovector, QuantityNature.Scalar) =>
                (QuantityNature.Pseudovector, OperationType.ScalarMultiply),

            // Vector × Vector = Scalar (tretiramo kao Dot product kad koristimo operator *)
            // Za Cross, korisnik mora pozvati .Cross() eksplicitno
            (QuantityNature.Vector, QuantityNature.Vector) =>
                (QuantityNature.Scalar, OperationType.DotProduct),

            // Default
            _ => (QuantityNature.Scalar, OperationType.ScalarMultiply)
        };
    }

    private static QuantityNature DetermineDivisionResult(QuantityNature a, QuantityNature b)
    {
        return (a, b) switch
        {
            // Vector / Scalar = Vector
            (QuantityNature.Vector, QuantityNature.Scalar) => QuantityNature.Vector,

            // Pseudovector / Scalar = Pseudovector
            (QuantityNature.Pseudovector, QuantityNature.Scalar) => QuantityNature.Pseudovector,

            // Anything else = Scalar
            _ => QuantityNature.Scalar
        };
    }
}