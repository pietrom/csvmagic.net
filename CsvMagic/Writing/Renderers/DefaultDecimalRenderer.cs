using System.Globalization;

namespace CsvMagic.Writing.Renderers;

public class DefaultDecimalRenderer : QuotableFieldRenderer<decimal?>
{
    protected override string RenderValue(CsvOptions options, decimal? value)
    {
        return value.HasValue ? value.Value.ToString(new NumberFormatInfo() { NumberDecimalSeparator = options.DecimalSeparator.ToString() }) : string.Empty;
    }
}