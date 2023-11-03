namespace CsvMagic.Reading.Parsers;

public class DefaultStringParser : QuotingParser<string?>
{
    protected override string? ParseValue(string? value)
    {
        return value;
    }
}