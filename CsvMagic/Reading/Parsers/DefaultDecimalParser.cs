namespace CsvMagic.Reading.Parsers;

public class DefaultDecimalParser : FieldParser
{
    public object? Parse(CsvOptions options, string? text)
    {
        return text == null ? null : decimal.Parse(text);
    }
}