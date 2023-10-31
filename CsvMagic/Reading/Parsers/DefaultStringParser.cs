namespace CsvMagic.Reading.Parsers;

public class DefaultStringParser : FieldParser
{
    private const char DoubleQuote = '"';

    public object? Parse(string? text)
    {
        return string.IsNullOrEmpty(text) ? string.Empty : Unquote(text);
    }

    private string Unquote(string text)
    {
        if (text[0] == DoubleQuote && text[^1] == DoubleQuote)
        {
            return text.Substring(1, text.Length - 2);
        }

        return text;
    }
}