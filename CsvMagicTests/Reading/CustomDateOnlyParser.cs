using CsvMagic;
using CsvMagic.Reading;
using CsvMagic.Reading.Parsers;

namespace CsvMagicTests.Reading;

public class CustomDateOnlyParser : QuotingParser<DateOnly> {
    protected override DateOnly ParseValue(CsvReadingContext context, string value) {
        return DateOnly.ParseExact(value, "yyyyMMdd");
    }
}
