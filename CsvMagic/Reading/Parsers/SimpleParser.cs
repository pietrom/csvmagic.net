namespace CsvMagic.Reading.Parsers;

public abstract class SimpleParser<T> : FieldParser
{
    public (object?, string?) ParseNext(CsvReadingContext context, string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return (null, null);
        }

        var index = text.IndexOf(context.Options.Delimiter);
        var value = index < 0 ? text : text.Substring(0, index);
        return (SafeParseValue(context, value), text.Substring(index + 1));
    }

    protected abstract T ParseValue(CsvReadingContext context, string value);

    private T SafeParseValue(CsvReadingContext context, string value)
    {
        try
        {
            return ParseValue(context, value);
        }
        catch (Exception ex)
        {
            throw new CsvReadingException(ex, context)
            {
                TokenText = value,
                ParserTag = GetType().Name
            };
        }
    }
}