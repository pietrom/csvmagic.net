namespace CsvMagic.Reading.Parsers;

public class DefaultLongParser : QuotingParser<long> {
    protected override long ParseValue(CsvReadingContext context, string value) {
        return long.Parse(value);
    }
}
