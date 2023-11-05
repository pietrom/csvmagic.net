namespace CsvMagic.Reading.Parsers;

public class DefaultDateOnlyParser : QuotingParser<DateOnly?>
{
    protected override DateOnly? ParseValue(CsvOptions options, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return DateOnly.ParseExact(value, "yyyy-MM-dd");
    }
}