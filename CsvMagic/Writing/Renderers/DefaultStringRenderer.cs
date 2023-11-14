namespace CsvMagic.Writing.Renderers;

public class DefaultStringRenderer : QuotableFieldRenderer<string> {
    protected override string RenderValue(CsvWritingContext context, string? value) {
        return value ?? string.Empty;
    }
}
