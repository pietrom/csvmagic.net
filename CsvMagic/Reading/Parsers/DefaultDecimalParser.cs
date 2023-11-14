using System.Globalization;

namespace CsvMagic.Reading.Parsers;

public class DefaultDecimalParser : QuotingParser<decimal> {
    protected override decimal ParseValue(CsvReadingContext context, string value) {
        return decimal.Parse(value, new NumberFormatInfo() { NumberDecimalSeparator = context.Options.DecimalSeparator.ToString() });
    }
}
