namespace CsvMagic.Reading.Parsers;

public class DefaultDateOnlyParser : QuotingParser<DateOnly?> {
    protected override DateOnly? ParseValue(CsvReadingContext context, string value) {
        if (string.IsNullOrEmpty(value)) {
            return null;
        }

        return DateOnly.ParseExact(value, "yyyy-MM-dd");
    }
}
