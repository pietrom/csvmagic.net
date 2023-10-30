using System.Reflection;
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
        _csvRowAttr = AttributeHelper.GetCsvRowAttribute(typeof(TRow)) ?? throw new System.Exception($"{typeof(TRow).Name} should be annotated with [CsvRow] attribute");
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

    public async IAsyncEnumerable<TRow> Read(StreamReader reader)
    {
        if (_csvRowAttr.HandleHeaderRow)
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

    private IEnumerable<string> GetLineFields(string? line)
    {
        if (line == null)
        {
            return Enumerable.Empty<string>();
        }

        return line.Split(_csvRowAttr.Delimiter);
    }

    private string Sanitize(string text)
    {
        return text.Contains(_csvRowAttr.Delimiter) ? $"\"{text}\"" : text;
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