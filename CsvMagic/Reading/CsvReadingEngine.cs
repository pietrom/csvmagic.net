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

    public async IAsyncEnumerable<TRow> Read(CsvOptions options, StreamReader reader)
    {
        var context = new CsvReadingContext(options, parsers, reader);

        if (options.HandleHeaderRow)
        {
            await context.NextLine();
        }

        while (context.HasMoreLines())
        {
            var line = await context.NextLine();
            var rest = line;

            object? row;
            try
            {
                (row, _) =  rootParser.ParseNext(context, rest);
            }
            catch (Exception ex)
            {
                throw new CsvReadingException(ex)
                {
                    ErrorLineNumber = context.LastReadLineNumber,
                    ErrorLineText = context.LastReadLine
                };
            }

            yield return (TRow) row;
        }
    }
}