namespace CsvMagic.Reading;

public class CsvReadingContext
{
    public CsvOptions Options { get; }
    private readonly IReadOnlyDictionary<Type, FieldParser> parsers;

    public CsvReadingContext(CsvOptions options, IReadOnlyDictionary<Type, FieldParser> parsers)
    {
        this.parsers = parsers;
        Options = options;
    }
}