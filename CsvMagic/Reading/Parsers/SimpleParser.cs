namespace CsvMagic.Reading.Parsers;

public abstract class SimpleParser<T> : FieldParser
{
    public (object?, string?) ParseNext(CsvOptions options, string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return (null, null);
        }

        var index = text.IndexOf(options.Delimiter);
        var value = index < 0 ? text : text.Substring(0, index);
        return (ParseValue(options, value), text.Substring(index + 1));
    }

    protected abstract T ParseValue(CsvOptions options, string value);
}