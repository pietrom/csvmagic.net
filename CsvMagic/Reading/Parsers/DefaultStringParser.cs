namespace CsvMagic.Reading.Parsers;

public class DefaultStringParser : FieldParser
{
    private const char DoubleQuote = '"';

    public object? Parse(string? text)
    {
        return string.IsNullOrEmpty(text) ? string.Empty : text;
    }

    private string Unquote(string text)
    {
        if (text[0] == DoubleQuote && text.Length > 1 && text[^1] == DoubleQuote)
        {
            return text.Substring(1, text.Length - 2).Replace("\"\"", "\"");
        }

        return text;
    }
}