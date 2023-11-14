using System.Globalization;

namespace CsvMagic.Writing.Renderers;

public class DefaultBooleanRenderer : QuotableFieldRenderer<bool?> {
    private readonly string trueText;
    private readonly string falseText;

    public DefaultBooleanRenderer(string trueText, string falseText) {
        this.trueText = trueText;
        this.falseText = falseText;
    }

    public DefaultBooleanRenderer() : this("1", "0") {
    }

    protected override string RenderValue(CsvWritingContext context, bool? value) {
        return value.HasValue ? value.Value ? trueText : falseText : string.Empty;
    }
}
