using System.Globalization;

namespace CsvMagic.Reading.Parsers;

public class DefaultDoubleParser : QuotingParser<double>
{
    protected override double ParseValue(CsvReadingContext context, string value)
    {
        return double.Parse(value, new NumberFormatInfo() { NumberDecimalSeparator = context.Options.DecimalSeparator.ToString() });
    }
}