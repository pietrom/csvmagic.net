namespace CsvMagic.Reading.Parsers;

public class DefaultStringParser : FieldParser
{
    public object? Parse(string? text)
    {
        return string.IsNullOrEmpty(text) ? string.Empty : text;
    }
}