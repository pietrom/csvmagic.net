namespace CsvMagic.Reading.Parsers;

public class DefaultIntParser : SimpleParser<int>
{
    protected override int ParseValue(CsvOptions options, string value)
    {
        return int.Parse(value);
    }
}