using System.Globalization;

namespace CsvMagic.Writing.Renderers;

public class DefaultDoubleRenderer : QuotableFieldRenderer<double?> {
    protected override string RenderValue(CsvWritingContext context, double? value) {
        return value.HasValue ? value.Value.ToString(new NumberFormatInfo() { NumberDecimalSeparator = context.Options.DecimalSeparator.ToString() }) : string.Empty;
    }
}
