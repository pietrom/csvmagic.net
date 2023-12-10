using System.Globalization;

namespace CsvMagic.Reading.Parsers;

public class DefaultFloatParser : QuotingParser<float> {
    protected override float ParseValue(CsvReadingContext context, string value) {
        return float.Parse(value, new NumberFormatInfo() { NumberDecimalSeparator = context.Options.DecimalSeparator.ToString() });
    }
}
