namespace CsvMagic.Writing.Renderers;

public class DefaultDateOnlyRenderer : QuotableFieldRenderer<DateOnly?>
{
    protected override string RenderValue(CsvOptions options, DateOnly? value)
    {
        return value.HasValue ? value.Value.ToString("yyyy-MM-dd") : string.Empty;
    }
}