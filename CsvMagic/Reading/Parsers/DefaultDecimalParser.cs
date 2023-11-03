namespace CsvMagic.Reading.Parsers;

public class DefaultDecimalParser : SimpleParser<decimal>
{
    protected override decimal ParseValue(string value)
    {
        return decimal.Parse(value);
    }
}