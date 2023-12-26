namespace CsvMagic.Reading.Parsers;

public class DefaultIntParser : QuotingParser<int> {
    protected override int ParseValue(CsvReadingContext context, string value) {
        return int.Parse(value);
    }
}
