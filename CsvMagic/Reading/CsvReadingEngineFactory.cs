using CsvMagic.Helpers;
using CsvMagic.Reading.Parsers;

namespace CsvMagic.Reading;

public class CsvReadingEngineFactory {
    private readonly IDictionary<Type, FieldParser> _defaultParsers = new Dictionary<Type, FieldParser>()
    {
        { typeof(string), new DefaultStringParser()},
        { typeof(int), new DefaultIntParser()},
        { typeof(int?), new DefaultIntParser()},
        { typeof(short), new DefaultShortParser()},
        { typeof(short?), new DefaultShortParser()},
        { typeof(long), new DefaultLongParser()},
        { typeof(long?), new DefaultLongParser()},
        { typeof(uint), new DefaultUintParser()},
        { typeof(uint?), new DefaultUintParser()},
        { typeof(decimal), new DefaultDecimalParser()},
        { typeof(decimal?), new DefaultDecimalParser()},
        { typeof(double), new DefaultDoubleParser()},
        { typeof(double?), new DefaultDoubleParser()},
        { typeof(float), new DefaultFloatParser()},
        { typeof(float?), new DefaultFloatParser()},
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

    public CsvReadingEngine<TRow> Create<TRow>() where TRow : new() => new SimpleCsvReadingEngine<TRow>(_defaultParsers.AsReadOnly(), new EmptyConstructorRowFactory<TRow>());

    public CsvReadingEngine<TRow> Create<TRow>(RowFactoryDelegate<TRow> factory) => new SimpleCsvReadingEngine<TRow>(_defaultParsers.AsReadOnly(), new RowFactoryDelegateWrapper<TRow>(factory));
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
