using System.Reflection;

namespace CsvMagic.Writing.Renderers;

internal class DefaultRenderer : FieldRenderer {
    public string RenderObject(CsvWritingContext context, object? value) {
        return value?.ToString() ?? string.Empty;
    }

    public IEnumerable<string> RenderHeader(CsvWritingContext context, PropertyInfo? propertyInfo = null) {
        return new[] { propertyInfo != null ? propertyInfo.Name : string.Empty };
    }
}
