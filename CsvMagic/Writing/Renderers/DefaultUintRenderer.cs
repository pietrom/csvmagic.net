namespace CsvMagic.Writing.Renderers;

public class DefaultUintRenderer : QuotableFieldRenderer<uint?> {
    protected override string RenderValue(CsvWritingContext context, uint? value) {
        return value.HasValue ? value.Value.ToString() : string.Empty;
    }
}
