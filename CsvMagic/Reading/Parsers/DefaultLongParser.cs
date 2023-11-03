namespace CsvMagic.Reading.Parsers;

public class DefaultLongParser : SimpleParser<long>
{
    protected override long ParseValue(string value)
    {
        return long.Parse(value);
    }
}