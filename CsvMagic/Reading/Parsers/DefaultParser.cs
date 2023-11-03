namespace CsvMagic.Reading.Parsers;

public class DefaultParser : SimpleParser<string>
{
    protected override string ParseValue(string value)
    {
        return value;
    }
}