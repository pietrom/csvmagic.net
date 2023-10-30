namespace CsvMagic.Reading.Parsers;

public class DefaultLongParser : FieldParser
{
    public object? Parse(string? text)
    {
        return text == null ? null : long.Parse(text);
    }
}