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

    public static DimensionalFormula operator *(DimensionalFormula a, DimensionalFormula b) =>
        new(
            mass: a._mass + b._mass,
            length: a._length + b._length,
            time: a._time + b._time,
            current: a._current + b._current,
            temperature: a._temperature + b._temperature,
            amount: a._amount + b._amount,
            luminous: a._luminous + b._luminous
        );

    public static DimensionalFormula operator /(in DimensionalFormula a, in DimensionalFormula b) =>
        new(
            mass: a._mass - b._mass,
            length: a._length - b._length,
            time: a._time - b._time,
            current: a._current - b._current,
            temperature: a._temperature - b._temperature,
            amount: a._amount - b._amount,
            luminous: a._luminous - b._luminous
        );

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

        Add("Mass", _mass);
        Add("Length", _length);
        Add("Time", _time);
        Add("ElectricCurrent", _current);
        Add("Temperature", _temperature);
        Add("AmountOfSubstance", _amount);
        Add("LuminousIntensity", _luminous);

        return parts.Count == 0 ? "Dimensionless" : string.Join(" ", parts);

        void Add(string name, sbyte exponent)
        {
            if (exponent == 0) return;
            parts.Add(exponent == 1 ? name : $"{name}^{exponent}");
        }
    }
}
