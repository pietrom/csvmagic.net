using System.Reflection;
using CsvMagic.Reading.Parsers;
using CsvMagic.Reflection;

namespace CsvMagic.Reading;

public class CsvReadingEngine<TRow>
{
    private readonly Func<TRow> rowFactory;
    private static readonly FieldParser DefaultParser = new DefaultParser();
    private readonly IReadOnlyList<(PropertyInfo, FieldParser)> metadata;
    private readonly CsvOptions? options;

    internal CsvReadingEngine(IReadOnlyDictionary<Type, FieldParser> parsers, Func<TRow> rowFactory)
    {
        this.rowFactory = rowFactory;
        metadata = InitParsers(parsers);
        options = AttributeHelper.GetCsvRowAttribute(typeof(TRow))?.Options;
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

    public async IAsyncEnumerable<TRow> Read(StreamReader reader, CsvOptions? localOptions = null)
    {
        var actualOptions = localOptions ?? options ?? throw new Exception($"Please annotate {typeof(TRow).Name} with [CsvRow] attribute or provide explicit CsvOptions parameter");
        if (actualOptions.HandleHeaderRow)
        {
            await reader.ReadLineAsync();
        }

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var row = rowFactory();
            var rest = line;

            foreach (var (info, parser) in metadata)
            {
                (var value, rest) = parser.ParseNext(actualOptions, rest);
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