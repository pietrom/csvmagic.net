namespace CsvMagic.Reading.Parsers;

public abstract class QuotingParser<T> : FieldParser
{
    protected abstract T ParseValue(CsvReadingContext context, string? value);

    public (object?, string?) ParseNext(CsvReadingContext context, string? text)
    {
        return GetNextAndRest(context, text);
    }

    private (T?, string?) GetNextAndRest(CsvReadingContext context, string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return (ParseValue(context, text), null);
        }

        if (text[0] != context.Options.Quoting)
        {
            var firstDelimiter = text.IndexOf(context.Options.Delimiter);
            var next = firstDelimiter > 0 ? text.Substring(0, firstDelimiter)
                : firstDelimiter < 0 ? text : string.Empty;

            var rest = firstDelimiter >= 0 ? text.Substring(firstDelimiter + 1) : null;
            return (ParseValue(context, next), rest);
        }

        var nextIndex = 1;
        var qoutesInARow = 0;
        bool go = true;
        while (nextIndex < text.Length && go)
        {
            if (text[nextIndex] == context.Options.Delimiter && qoutesInARow > 0)
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
            else if (text[nextIndex] == context.Options.Quoting)
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
            ? (ParseValue(context, Sanitize(context.Options, text.Substring(1, nextIndex - 2))), text.Substring(nextIndex + 1))
            : (ParseValue(context, Sanitize(context.Options, text.Substring(1, text.Length - 2))), text.Last() == context.Options.Delimiter ? string.Empty : null);
    }

    private string Sanitize(CsvOptions options, string text)
    {
        return text.Replace($"{options.Quoting}{options.Quoting}", $"{options.Quoting}");
    }
}