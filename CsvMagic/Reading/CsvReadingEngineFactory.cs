using CsvMagic.Reading.Parsers;

namespace CsvMagic.Reading;

public class CsvReadingEngineFactory {
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

    public CsvReadingEngineFactory RegisterParser<TField>(FieldParser parser) {
        _defaultParsers[typeof(TField)] = parser;
        return this;
    }

    public CsvReadingEngine<TRow> Create<TRow>() where TRow : new() => new CsvReadingEngine<TRow>(_defaultParsers.AsReadOnly(), new EmptyConstructorRowFactory<TRow>());

    public CsvReadingEngine<TRow> Create<TRow>(RowFactoryDelegate<TRow> factory) => new CsvReadingEngine<TRow>(_defaultParsers.AsReadOnly(), new RowFactoryDelegateWrapper<TRow>(factory));

    public CsvReadingEngine<TRow> Create<TRow>(RowFactory factory) => new CsvReadingEngine<TRow>(_defaultParsers.AsReadOnly(), factory);
}

public interface RowFactory {
    object? Create(IDictionary<string, object?> fields);
}

public delegate TRow RowFactoryDelegate<TRow>(IDictionary<string, object?> fields);

public class EmptyConstructorRowFactory<TRow> : RowFactory where TRow : new() {
    public object Create(IDictionary<string, object?> fields) {
        var row = new TRow();
        var type = typeof(TRow);
        foreach (var (key, value) in fields) {
            type.GetProperty(key).SetValue(row, value);
        }
        return row;
    }
}

public class RowFactoryDelegateWrapper<T> : RowFactory {
    private readonly RowFactoryDelegate<T> factory;

    public RowFactoryDelegateWrapper(RowFactoryDelegate<T> factory) {
        this.factory = factory;
    }

    public object? Create(IDictionary<string, object?> fields) {
        return factory(fields);
    }
}
