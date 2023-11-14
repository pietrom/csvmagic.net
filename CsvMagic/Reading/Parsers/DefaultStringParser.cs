namespace CsvMagic.Reading.Parsers;

public class DefaultStringParser : QuotingParser<string?> {
    protected override string? ParseValue(CsvReadingContext context, string? value) {
        return value;
    }
}
