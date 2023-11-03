namespace CsvMagic.Reading.Parsers;

public class DefaultIntParser : FieldParser
{
    public object? Parse(CsvOptions options, string? text)
    {
        return text == null ? null : int.Parse(text);
    }
}