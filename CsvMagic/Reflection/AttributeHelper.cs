using System.Reflection;

namespace CsvMagic.Reflection;

public static class AttributeHelper {
    public static CsvField? GetCsvFieldAttribute(PropertyInfo from) {
        return from.GetCustomAttributes(typeof(CsvField), true).SingleOrDefault() as CsvField;
    }
}
