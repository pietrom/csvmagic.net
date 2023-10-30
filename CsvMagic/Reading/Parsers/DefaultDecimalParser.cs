namespace CsvMagic.Reading.Parsers;

public class DefaultDecimalParser : FieldParser
{
    public object? Parse(string? text)
    {
        return text == null ? null : decimal.Parse(text);
    }
}