namespace CsvMagic.Reading.Parsers;

public class DefaultLongParser : SimpleParser<long>
{
    protected override long ParseValue(CsvReadingContext context, string value)
    {
        return long.Parse(value);
    }
}