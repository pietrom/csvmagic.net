namespace CsvMagic.Reading.Parsers;

public class DefaultParser : FieldParser
{
    public object? Parse(string? text)
    {
        return text;
    }
}