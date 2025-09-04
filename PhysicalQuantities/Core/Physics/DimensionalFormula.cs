namespace PhysicalQuantities.Core.Physics;

public readonly struct DimensionalFormula(
    int mass = 0,
    int length = 0,
    int time = 0,
    int current = 0,
    int temperature = 0,
    int amount = 0,
    int luminous = 0)
    : IEquatable<DimensionalFormula>
{
    private readonly sbyte _mass = (sbyte)mass,
        _length = (sbyte)length,
        _time = (sbyte)time,
        _current = (sbyte)current,
        _temperature = (sbyte)temperature,
        _amount = (sbyte)amount,
        _luminous = (sbyte)luminous;

    private static DimensionalFormula Dimensionless => new();

    public static DimensionalFormula operator *(DimensionalFormula a, DimensionalFormula b)
        => Combine(a, b, add: true);

    public static DimensionalFormula operator /(DimensionalFormula a, DimensionalFormula b)
        => Combine(a, b, add: false);

    private static DimensionalFormula Combine(DimensionalFormula a, DimensionalFormula b, bool add)
    {
        return new DimensionalFormula(
            mass: add ? a._mass + b._mass : a._mass - b._mass,
            length: add ? a._length + b._length : a._length - b._length,
            time: add ? a._time + b._time : a._time - b._time,
            current: add ? a._current + b._current : a._current - b._current,
            temperature: add ? a._temperature + b._temperature : a._temperature - b._temperature,
            amount: add ? a._amount + b._amount : a._amount - b._amount,
            luminous: add ? a._luminous + b._luminous : a._luminous - b._luminous
        );
    }

    public DimensionalFormula RaiseToPower(int exponent)
    {
        if (exponent == 1) return this;
        if (exponent == 0) return Dimensionless;

        return new DimensionalFormula(
            mass: _mass * exponent,
            length: _length * exponent,
            time: _time * exponent,
            current: _current * exponent,
            temperature: _temperature * exponent,
            amount: _amount * exponent,
            luminous: _luminous * exponent
        );
    }

    public bool Equals(DimensionalFormula other)
        => _mass == other._mass && _length == other._length && _time == other._time && _current == other._current
           && _temperature == other._temperature && _amount == other._amount && _luminous == other._luminous;

    public override bool Equals(object? obj) => obj is DimensionalFormula o && Equals(o);

    public override int GetHashCode()
    {
        var h = new HashCode();
        h.Add(_mass); h.Add(_length); h.Add(_time); h.Add(_current); h.Add(_temperature); h.Add(_amount); h.Add(_luminous);
        return h.ToHashCode();
    }

    public override string ToString()
    {
        var parts = new List<string>(7);
        if (_mass != 0) parts.Add(_mass == 1 ? "Mass" : $"Mass^{_mass}");
        if (_length != 0) parts.Add(_length == 1 ? "Length" : $"Length^{_length}");
        if (_time != 0) parts.Add(_time == 1 ? "Time" : $"Time^{_time}");
        if (_current != 0) parts.Add(_current == 1 ? "ElectricCurrent" : $"ElectricCurrent^{_current}");
        if (_temperature != 0) parts.Add(_temperature == 1 ? "Temperature" : $"Temperature^{_temperature}");
        if (_amount != 0) parts.Add(_amount == 1 ? "AmountOfSubstance" : $"AmountOfSubstance^{_amount}");
        if (_luminous != 0) parts.Add(_luminous == 1 ? "LuminousIntensity" : $"LuminousIntensity^{_luminous}");

        return parts.Count == 0 ? "Dimensionless" : string.Join(" ", parts);
    }
}