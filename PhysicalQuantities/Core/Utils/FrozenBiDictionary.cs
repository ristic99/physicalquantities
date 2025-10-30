using System.Collections.Frozen;

namespace PhysicalQuantities.Core.Utils;

public sealed class FrozenBiDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    public FrozenDictionary<TKey, TValue> Forward { get; }
    public FrozenDictionary<TValue, TKey> Reverse { get; }

    public FrozenBiDictionary(IDictionary<TKey, TValue> dictionary)
    {
        Forward = dictionary.ToFrozenDictionary();
        Reverse = dictionary.ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
            .ToFrozenDictionary();
    }

    public bool TryGetByKey(TKey key, out TValue? value) =>
        Forward.TryGetValue(key, out value);

    public bool TryGetByValue(TValue value, out TKey? key) =>
        Reverse.TryGetValue(value, out key);

    public TValue this[TKey key] => Forward[key];
    public TKey this[TValue value] => Reverse[value];
}