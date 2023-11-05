namespace CsvMagic.Reading.Parsers;

public class DefaultLongParser : SimpleParser<long>
{
    protected override long ParseValue(CsvOptions options, string value)
    {
        return long.Parse(value);
    }
}