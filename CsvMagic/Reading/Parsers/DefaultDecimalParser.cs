using System.Globalization;

namespace CsvMagic.Reading.Parsers;

public class DefaultDecimalParser : SimpleParser<decimal>
{
    protected override decimal ParseValue(CsvOptions options, string value)
    {
        return decimal.Parse(value, new NumberFormatInfo() { NumberDecimalSeparator = options.DecimalSeparator.ToString() });
    }
}