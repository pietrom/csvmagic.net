using System.Reflection;
using CsvMagic.Reading.Parsers;
using CsvMagic.Reflection;

namespace CsvMagic.Reading;

public class CsvReadingEngine<TRow>
{
    private readonly IReadOnlyDictionary<Type, FieldParser> parsers;
    private readonly Func<TRow> rowFactory;
    private static readonly FieldParser DefaultParser = new DefaultParser();
    private readonly IReadOnlyList<(PropertyInfo, FieldParser)> metadata;

    internal CsvReadingEngine(IReadOnlyDictionary<Type, FieldParser> parsers, Func<TRow> rowFactory)
    {
        this.parsers = parsers;
        this.rowFactory = rowFactory;
        metadata = InitParsers(parsers);
    }

    private IReadOnlyList<(PropertyInfo, FieldParser)> InitParsers(
        IReadOnlyDictionary<Type, FieldParser> parsers)
    {
        return ReflectionHelper.GetTypeProperties(typeof(TRow))
            .Where(p => p.CanWrite)
            .Select(
                p => (p, GetParserFor(p) ?? (parsers.ContainsKey(p.PropertyType)
                            ? parsers[p.PropertyType]
                            : DefaultParser)
                    )
            ).ToList();
    }

    public async IAsyncEnumerable<TRow> Read(StreamReader reader, CsvOptions options)
    {
        if (options.HandleHeaderRow)
        {
            await reader.ReadLineAsync();
        }

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var row = rowFactory();
            var rest = line;
            var context = new CsvReadingContext(options, parsers);

            foreach (var (info, parser) in metadata)
            {
                (var value, rest) = parser.ParseNext(context, rest);
                info.SetValue(row, value);
            }

            yield return row;
        }
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