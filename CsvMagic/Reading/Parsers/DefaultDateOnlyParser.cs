namespace CsvMagic.Reading.Parsers;

public class DefaultDateOnlyParser : SimpleParser<DateOnly>
{
    protected override DateOnly ParseValue(string value)
    {
        return DateOnly.ParseExact(value, "yyyy-MM-dd");
    }
}