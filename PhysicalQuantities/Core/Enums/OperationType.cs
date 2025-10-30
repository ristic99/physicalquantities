namespace PhysicalQuantities.Core.Enums;

public enum OperationType
{
    Direct,        // Created directly by user
    ScalarMultiply,// Scalar * Scalar or Scalar * Vector
    DotProduct,    // Vector · Vector → Scalar
    CrossProduct   // Vector × Vector → Pseudovector
}