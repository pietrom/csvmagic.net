namespace CsvMagic.Reading.Parsers;

public class DefaultDateOnlyParser : FieldParser
{
    public object? Parse(string? text)
    {
        return string.IsNullOrEmpty(text) ? null : DateOnly.ParseExact(text, "yyyy-MM-dd");
    }
}