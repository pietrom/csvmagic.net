namespace CsvMagic.Reading.Parsers;

public class DefaultLongParser : FieldParser
{
    public object? Parse(CsvOptions options, string? text)
    {
        return text == null ? null : long.Parse(text);
    }
}