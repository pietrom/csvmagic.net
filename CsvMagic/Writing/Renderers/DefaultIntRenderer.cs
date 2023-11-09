namespace CsvMagic.Writing.Renderers;

public class DefaultIntRenderer : QuotableFieldRenderer<int?>
{
    protected override string RenderValue(CsvWritingContext context, int? value)
    {
        return value.HasValue ? value.Value.ToString() : string.Empty;
    }
}