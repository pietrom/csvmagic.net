namespace CsvMagic.Writing.Renderers;

public class DefaultShortRenderer : QuotableFieldRenderer<short?> {
    protected override string RenderValue(CsvWritingContext context, short? value) {
        return value.HasValue ? value.Value.ToString() : string.Empty;
    }
}
