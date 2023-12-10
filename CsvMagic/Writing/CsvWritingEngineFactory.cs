using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public class CsvWritingEngineFactory {
    private readonly IDictionary<Type, FieldRenderer> _defaultRenderers = new Dictionary<Type, FieldRenderer>
    {
        { typeof(string), new DefaultStringRenderer() },
        { typeof(int), new DefaultIntRenderer() },
        { typeof(int?), new DefaultIntRenderer() },
        { typeof(short), new DefaultShortRenderer() },
        { typeof(short?), new DefaultShortRenderer() },
        { typeof(long), new DefaultLongRenderer() },
        { typeof(long?), new DefaultLongRenderer() },
        { typeof(uint), new DefaultUintRenderer() },
        { typeof(uint?), new DefaultUintRenderer() },
        { typeof(decimal), new DefaultDecimalRenderer()},
        { typeof(decimal?), new DefaultDecimalRenderer()},
        { typeof(double), new DefaultDoubleRenderer()},
        { typeof(double?), new DefaultDoubleRenderer()},
        { typeof(float), new DefaultFloatRenderer()},
        { typeof(float?), new DefaultFloatRenderer()},
        { typeof(bool), new DefaultBooleanRenderer()},
        { typeof(bool?), new DefaultBooleanRenderer()},
        { typeof(DateOnly), new DefaultDateOnlyRenderer()},
        { typeof(DateOnly?), new DefaultDateOnlyRenderer()},
        { typeof(DateTimeOffset), new DefaultDateTimeOffsetRenderer()},
        { typeof(DateTimeOffset?), new DefaultDateTimeOffsetRenderer()},
    };

    public CsvWritingEngineFactory RegisterRenderer<TField>(FieldRenderer renderer) {
        _defaultRenderers[typeof(TField)] = renderer;
        return this;
    }

    public CsvWritingEngine<TRow> Create<TRow>() => new CsvWritingEngine<TRow>(_defaultRenderers.AsReadOnly());
}
