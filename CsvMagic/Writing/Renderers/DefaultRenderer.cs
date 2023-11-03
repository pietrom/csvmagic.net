namespace CsvMagic.Writing.Renderers;

internal class DefaultRenderer : FieldRenderer
{
    public string Render(CsvOptions options, object? value)
    {
        return value?.ToString() ?? string.Empty;
    }
}