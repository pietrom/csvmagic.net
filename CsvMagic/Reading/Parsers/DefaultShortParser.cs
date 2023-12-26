namespace CsvMagic.Reading.Parsers;

public class DefaultShortParser : QuotingParser<short> {
    protected override short ParseValue(CsvReadingContext context, string value) {
        return short.Parse(value);
    }
}
