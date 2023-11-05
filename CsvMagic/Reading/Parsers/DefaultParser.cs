namespace CsvMagic.Reading.Parsers;

public class DefaultParser : SimpleParser<string>
{
    protected override string ParseValue(CsvOptions options, string value)
    {
        return value;
    }
}