namespace CsvMagic.Reading.Parsers;

public class DefaultDateOnlyParser : QuotingParser<DateOnly?> {
    private readonly string pattern;

    public DefaultDateOnlyParser(string pattern) {
        this.pattern = pattern;
    }

    public DefaultDateOnlyParser() : this("yyyy-MM-dd") { }

    protected override DateOnly? ParseValue(CsvReadingContext context, string value) {
        if (string.IsNullOrEmpty(value)) {
            return null;
        }

        return DateOnly.ParseExact(value, pattern);
    }
}
