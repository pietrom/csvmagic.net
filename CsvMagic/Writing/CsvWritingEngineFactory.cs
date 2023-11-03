using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public class CsvWritingEngineFactory
{
    private readonly IDictionary<Type, FieldRenderer> _defaultRenderers = new Dictionary<Type, FieldRenderer>
    {
        { typeof(string), new DefaultStringRenderer() },
    };

    public CsvWritingEngineFactory AddSerializer<TField>(FieldRenderer renderer)
    {
        _defaultRenderers.Add(typeof(TField), renderer);
        return this;
    }

    public CsvWritingEngine<TRow> Create<TRow>() => new CsvWritingEngine<TRow>(_defaultRenderers.AsReadOnly());
}