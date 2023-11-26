using System.Reflection;
using CsvMagic.Reflection;

namespace CsvMagic.Writing.Renderers;

public static class FieldRendererHelper {
    public static string GetLabelFor(PropertyInfo info) {
        var attr = AttributeHelper.GetCsvFieldAttribute(info);
        return attr != null && attr.Label != null ? attr.Label : info.Name;
    }
}
