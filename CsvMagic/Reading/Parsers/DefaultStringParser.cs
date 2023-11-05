namespace CsvMagic.Reading.Parsers;

public class DefaultStringParser : QuotingParser<string?>
{
    protected override string? ParseValue(CsvOptions options, string? value)
    {
        return value;
    }
}