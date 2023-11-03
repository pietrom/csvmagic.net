namespace CsvMagic.Reading.Parsers;

public abstract class QuotingParser<T> : FieldParser
{
    protected abstract T ParseValue(string? value);

    public (object?, string?) ParseNext(CsvOptions options, string? text)
    {
        return GetNextAndRest(options, text);
    }

    private (T?, string?) GetNextAndRest(CsvOptions options, string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return (ParseValue(text), null);
        }

        if (text[0] != options.Quoting)
        {
            var firstDelimiter = text.IndexOf(options.Delimiter);
            var next = firstDelimiter > 0 ? text.Substring(0, firstDelimiter)
                : firstDelimiter < 0 ? text : string.Empty;

            var rest = firstDelimiter >= 0 ? text.Substring(firstDelimiter + 1) : null;
            return (ParseValue(next), rest);
        }

        var nextIndex = 1;
        var qoutesInARow = 0;
        bool go = true;
        while (nextIndex < text.Length && go)
        {
            if (text[nextIndex] == options.Delimiter && qoutesInARow > 0)
            {
                if (qoutesInARow % 2 == 1)
                {
                    go = false;
                }
                else
                {
                    qoutesInARow = 0;
                    nextIndex++;
                }
            }
            else if (text[nextIndex] == options.Quoting)
            {
                qoutesInARow++;
                nextIndex++;
            }
            else
            {
                nextIndex++;
            }
        }

        return nextIndex <= text.Length - 1
            ? (ParseValue(Sanitize(options, text.Substring(1, nextIndex - 2))), text.Substring(nextIndex + 1))
            : (ParseValue(Sanitize(options, text.Substring(1, text.Length - 2))), text.Last() == options.Delimiter ? string.Empty : null);
    }

    private string Sanitize(CsvOptions options, string text)
    {
        return text.Replace($"{options.Quoting}{options.Quoting}", $"{options.Quoting}");
    }
}