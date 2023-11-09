namespace CsvMagic.Writing.Renderers;

public class DefaultLongRenderer : QuotableFieldRenderer<long?>
{
    protected override string RenderValue(CsvWritingContext context, long? value)
    {
        return value.HasValue ? value.Value.ToString() : string.Empty;
    }
}