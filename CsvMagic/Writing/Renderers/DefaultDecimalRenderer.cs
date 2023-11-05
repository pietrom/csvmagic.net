using System.Globalization;

namespace CsvMagic.Writing.Renderers;

public class DefaultDecimalRenderer : FieldRenderer
{
    public string Render(CsvOptions options, object? value)
    {
        var dec = value as decimal?;
        return dec.HasValue ? dec.Value.ToString(new NumberFormatInfo() { NumberDecimalSeparator = options.DecimalSeparator.ToString() }) : string.Empty;
    }
}