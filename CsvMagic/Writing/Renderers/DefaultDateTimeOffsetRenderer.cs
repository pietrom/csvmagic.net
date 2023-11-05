namespace CsvMagic.Writing.Renderers;

public class DefaultDateTimeOffsetRenderer : FieldRenderer
{
    public string Render(CsvOptions options, object? value)
    {
        var dateOnly = value as DateTimeOffset?;
        return dateOnly.HasValue ? dateOnly.Value.ToString("O") : string.Empty;
    }
}