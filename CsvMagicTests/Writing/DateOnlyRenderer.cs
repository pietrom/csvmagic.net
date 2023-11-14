using System.Reflection;
using CsvMagic;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

public class DateOnlyRenderer : FieldRenderer {
    private readonly string _format;

    public DateOnlyRenderer(string format = "yyyy-MM-dd") {
        _format = format;
    }

    public DateOnlyRenderer() : this("yyyy-MM-dd") {
    }

    public string RenderObject(CsvWritingContext context, object? value) {
        var dateOnlyValue = value as DateOnly?;
        return dateOnlyValue.HasValue ? dateOnlyValue.Value.ToString(_format) : string.Empty;
    }

    public string RenderHeader(CsvWritingContext context, PropertyInfo? propertyInfo = null) {
        return propertyInfo?.Name ?? string.Empty;
    }
}
