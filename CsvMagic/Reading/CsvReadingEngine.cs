using System.Reflection;
using CsvMagic.Reading.Parsers;
using CsvMagic.Reflection;

namespace CsvMagic.Reading;

public class CsvReadingEngine<TRow>
{
    private readonly Func<TRow> rowFactory;
    private static readonly FieldParser DefaultParser = new DefaultParser();
    private readonly IReadOnlyList<(PropertyInfo, FieldParser)> metadata;
    private readonly CsvOptions options;

    internal CsvReadingEngine(IReadOnlyDictionary<Type, FieldParser> parsers, Func<TRow> rowFactory)
    {
        this.rowFactory = rowFactory;
        metadata = InitParsers(parsers);
        options = AttributeHelper.GetCsvRowAttribute(typeof(TRow))?.Options ??
                      throw new System.Exception($"{typeof(TRow).Name} should be annotated with [CsvRow] attribute");
    }

    private IReadOnlyList<(PropertyInfo, FieldParser)> InitParsers(
        IReadOnlyDictionary<Type, FieldParser> parsers)
    {
        return ReflectionHelper.GetTypeProperties(typeof(TRow))
            .Select(
                p => (p, GetParserFor(p) ?? (parsers.ContainsKey(p.PropertyType)
                            ? parsers[p.PropertyType]
                            : DefaultParser)
                    )
            ).ToList();
    }

    public async IAsyncEnumerable<TRow> Read(StreamReader reader, bool? handleHeadersRow = null)
    {
        var hasHeader = handleHeadersRow.HasValue ? handleHeadersRow.Value : options.HandleHeaderRow;
        if (hasHeader)
        {
            await reader.ReadLineAsync();
        }

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var fields = GetLineFields(line);
            using var fieldsEnumerator = fields.GetEnumerator();
            var row = rowFactory();

            foreach (var (info, parser) in metadata)
            {
                if (fieldsEnumerator.MoveNext())
                {
                    var text = fieldsEnumerator.Current;
                    info.SetValue(row, parser.Parse(options, text));
                }
            }

            yield return row;
        }
    }

    private IEnumerable<string> GetLineFields(string? line)
    {
        string? rest = line ?? string.Empty;
        while (rest != null)
        {
            var (n, r) = GetNextAndRest(rest);
            rest = r;
            yield return n;
        }
    }

    private (string, string?) GetNextAndRest(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return (string.Empty, null);
        }

        if (text[0] != options.Quoting)
        {
            var firstDelimiter = text.IndexOf(options.Delimiter);
            var next = firstDelimiter > 0 ? text.Substring(0, firstDelimiter)
                : firstDelimiter < 0 ? text : string.Empty;

            var rest = firstDelimiter >= 0 ? text.Substring(firstDelimiter + 1) : null;
            return (next, rest);
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
            ? (Sanitize(text.Substring(1, nextIndex - 2)), text.Substring(nextIndex + 1))
            : (Sanitize(text.Substring(1, text.Length - 2)), text.Last() == options.Delimiter ? string.Empty : null);
    }

    private string Sanitize(string text)
    {
        return text.Replace($"{options.Quoting}{options.Quoting}", $"{options.Quoting}");
    }

    private FieldParser? GetParserFor(PropertyInfo p)
    {
        var fieldAttr = AttributeHelper.GetCsvFieldAttribute(p);
        FieldParser? parser = null;
        if (fieldAttr != null && fieldAttr.Parser != null)
        {
            parser = (FieldParser?)Activator.CreateInstance(fieldAttr.Parser);
        }

        return parser;
    }
}