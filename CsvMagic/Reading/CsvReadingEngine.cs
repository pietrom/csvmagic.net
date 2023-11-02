using System.Reflection;
using System.Text.RegularExpressions;
using CsvMagic.Reading.Parsers;
using CsvMagic.Reflection;

namespace CsvMagic.Reading;

public class CsvReadingEngine<TRow>
{
    private readonly Func<TRow> _rowFactory;
    private static readonly FieldParser DefaultParser = new DefaultParser();
    private readonly IReadOnlyList<(PropertyInfo, FieldParser)> _metadata;
    private readonly CsvRow _csvRowAttr;

    internal CsvReadingEngine(IReadOnlyDictionary<Type, FieldParser> parsers, Func<TRow> rowFactory)
    {
        _rowFactory = rowFactory;
        _metadata = InitParsers(parsers);
        _csvRowAttr = AttributeHelper.GetCsvRowAttribute(typeof(TRow)) ??
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
        var hasHeader = handleHeadersRow.HasValue ? handleHeadersRow.Value : _csvRowAttr.HandleHeaderRow;
        if (hasHeader)
        {
            await reader.ReadLineAsync();
        }

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var fields = GetLineFields(line);
            using var fieldsEnumerator = fields.GetEnumerator();
            var row = _rowFactory();

            foreach (var (info, parser) in _metadata)
            {
                fieldsEnumerator.MoveNext();
                var text = fieldsEnumerator.Current;
                info.SetValue(row, parser.Parse(text));
            }

            yield return row;
        }
    }

    private const char DoubleQuote = '"'; // TODO duplicated

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

        if (text[0] != DoubleQuote)
        {
            var firstDelimiter = text.IndexOf(_csvRowAttr.Delimiter);
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
            if (text[nextIndex] == _csvRowAttr.Delimiter && qoutesInARow > 0)
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
            else if (text[nextIndex] == DoubleQuote)
            {
                qoutesInARow++;
                nextIndex++;
            }
            else
            {nextIndex++;
            }
        }

        return nextIndex < text.Length - 2
            ? (Sanitize(text.Substring(1, nextIndex - 2)), text.Substring(nextIndex + 1))
            : (Sanitize(text.Substring(1, text.Length - 2)), null);
    }

    private string Sanitize(string text)
    {
        return text.Replace($"{DoubleQuote}{DoubleQuote}", $"{DoubleQuote}");
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