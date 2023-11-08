namespace CsvMagic.Reading.Parsers;

public class DefaultIntParser : SimpleParser<int>
{
    protected override int ParseValue(CsvReadingContext context, string value)
    {
        return int.Parse(value);
    }
}