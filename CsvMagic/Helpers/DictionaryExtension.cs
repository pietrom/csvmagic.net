namespace CsvMagic.Helpers;

public static class DictionaryExtension {
    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue) {
        return self.TryGetValue(key, out var value) ? value : defaultValue;
    }

    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<TValue> defaultValueProducer) {
        return self.TryGetValue(key, out var value) ? value : defaultValueProducer();
    }
}
