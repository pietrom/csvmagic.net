using System.Reflection;

namespace CsvMagic.Reflection;

public static class ReflectionHelper {
    public static IEnumerable<PropertyInfo> GetTypeProperties(Type? t) {
        if (t == null) {
            return Enumerable.Empty<PropertyInfo>();
        }

        return GetTypeProperties(t.BaseType)
            .Concat(t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly));
    }
}
