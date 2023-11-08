using CsvMagic.Reading.Parsers;

namespace CsvMagic.Reading;

public class CsvReadingEngine<TRow> where TRow : new()
{
    private readonly IReadOnlyDictionary<Type, FieldParser> parsers;
    private readonly FieldParser rootParser;

    internal CsvReadingEngine(IReadOnlyDictionary<Type, FieldParser> parsers)
    {
        this.parsers = parsers;
        rootParser = new ComplexTypeParser<TRow>();
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

            var rest = line;
            var context = new CsvReadingContext(options, parsers);

            var (row, _) =  rootParser.ParseNext(context, rest);
            yield return (TRow) row;
        }
    }
}