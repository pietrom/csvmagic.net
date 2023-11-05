namespace CsvMagic.Writing.Renderers;

public class DefaultStringRenderer : QuotableFieldRenderer<string>
{
    protected override string RenderValue(CsvOptions options, string? value)
    {
        return value ?? string.Empty;
    }
}