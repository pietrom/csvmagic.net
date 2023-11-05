namespace CsvMagic.Writing.Renderers;

public class DefaultDateTimeOffsetRenderer : QuotableFieldRenderer<DateTimeOffset?>
{
    protected override string RenderValue(CsvOptions options, DateTimeOffset? value)
    {
        return value.HasValue ? value.Value.ToString("O") : string.Empty;
    }
}