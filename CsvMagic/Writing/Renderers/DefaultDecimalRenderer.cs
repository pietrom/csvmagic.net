using System.Globalization;

namespace CsvMagic.Writing.Renderers;

public class DefaultDecimalRenderer : QuotableFieldRenderer<decimal?> {
    protected override string RenderValue(CsvWritingContext context, decimal? value) {
        return value.HasValue ? value.Value.ToString(new NumberFormatInfo() { NumberDecimalSeparator = context.Options.DecimalSeparator.ToString() }) : string.Empty;
    }
}
