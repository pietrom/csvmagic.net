namespace CsvMagic.Writing.Renderers;

public class DefaultDateOnlyRenderer : FieldRenderer
{
    public string Render(CsvOptions options, object? value)
    {
        var dateOnly = value as DateOnly?;
        return dateOnly.HasValue ? dateOnly.Value.ToString("yyyy-MM-dd") : string.Empty;
    }
}