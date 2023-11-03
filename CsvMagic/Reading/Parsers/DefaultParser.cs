namespace CsvMagic.Reading.Parsers;

public class DefaultParser : FieldParser
{
    public object? Parse(CsvOptions options, string? text)
    {
        return text;
    }
}