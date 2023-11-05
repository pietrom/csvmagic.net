using CsvMagic.Reading.Parsers;

namespace CsvMagic.Reading;

public class CsvReadingEngineFactory
{
    private readonly IDictionary<Type, FieldParser> _defaultParsers = new Dictionary<Type, FieldParser>()
    {
        { typeof(string), new DefaultStringParser()},
        { typeof(int), new DefaultIntParser()},
        { typeof(int?), new DefaultIntParser()},
        { typeof(long), new DefaultLongParser()},
        { typeof(long?), new DefaultLongParser()},
        { typeof(decimal), new DefaultDecimalParser()},
        { typeof(decimal?), new DefaultDecimalParser()},
        { typeof(DateOnly), new DefaultDateOnlyParser()},
        { typeof(DateOnly?), new DefaultDateOnlyParser()},
    };

    public CsvReadingEngineFactory AddSerializer<TField>(FieldParser parser)
    {
        _defaultParsers[typeof(TField)] = parser;
        return this;
    }

    public CsvReadingEngine<TRow> Create<TRow>(Func<TRow> rowFactory) => new CsvReadingEngine<TRow>(_defaultParsers.AsReadOnly(), rowFactory);

    public CsvReadingEngine<TRow> Create<TRow>() where TRow : new() => new CsvReadingEngine<TRow>(_defaultParsers.AsReadOnly(), () => new TRow());
}