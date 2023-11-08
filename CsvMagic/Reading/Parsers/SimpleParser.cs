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
        return (ParseValue(context, value), text.Substring(index + 1));
    }

    protected abstract T ParseValue(CsvReadingContext context, string value);
}