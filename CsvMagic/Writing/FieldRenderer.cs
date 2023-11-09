using System.Reflection;

namespace CsvMagic.Writing;

public interface FieldRenderer
{
    string RenderObject(CsvWritingContext context, object? value);

    string RenderHeader(CsvWritingContext context, PropertyInfo? propertyInfo = null);
}