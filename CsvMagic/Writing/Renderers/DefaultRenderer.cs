namespace CsvMagic.Writing.Renderers;

internal class DefaultRenderer : FieldRenderer
{
    public string Render(object? value)
    {
        return value?.ToString() ?? string.Empty;
    }
}