namespace CsvMagic.Reading.Parsers;

public class DefaultStringParser : FieldParser
{
    public object? Parse(CsvOptions options, string? text)
    {
        return string.IsNullOrEmpty(text) ? string.Empty : text;
    }
}