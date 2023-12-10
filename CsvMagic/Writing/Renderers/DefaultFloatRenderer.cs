using System.Globalization;

namespace CsvMagic.Writing.Renderers;

public class DefaultFloatRenderer : QuotableFieldRenderer<float?> {
    protected override string RenderValue(CsvWritingContext context, float? value) {
        return value.HasValue ? value.Value.ToString(new NumberFormatInfo() { NumberDecimalSeparator = context.Options.DecimalSeparator.ToString() }) : string.Empty;
    }
}
