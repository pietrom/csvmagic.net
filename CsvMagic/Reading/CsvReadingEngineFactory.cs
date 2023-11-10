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
        { typeof(double), new DefaultDoubleParser()},
        { typeof(double?), new DefaultDoubleParser()},
        { typeof(bool), new DefaultBooleanParser()},
        { typeof(bool?), new DefaultBooleanParser()},
        { typeof(DateOnly), new DefaultDateOnlyParser()},
        { typeof(DateOnly?), new DefaultDateOnlyParser()},
        { typeof(DateTimeOffset), new DefaultDateTimeOffsetParser()},
        { typeof(DateTimeOffset?), new DefaultDateTimeOffsetParser()},
    };

    public CsvReadingEngineFactory RegisterParser<TField>(FieldParser parser)
    {
        _defaultParsers[typeof(TField)] = parser;
        return this;
    }

    public CsvReadingEngine<TRow> Create<TRow>() where TRow : new() => new CsvReadingEngine<TRow>(_defaultParsers.AsReadOnly());
}