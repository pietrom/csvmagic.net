namespace CsvMagic.Writing.Renderers;

public class DefaultDateOnlyRenderer : QuotableFieldRenderer<DateOnly?> {
    protected override string RenderValue(CsvWritingContext context, DateOnly? value) {
        return value.HasValue ? value.Value.ToString("yyyy-MM-dd") : string.Empty;
    }
}
