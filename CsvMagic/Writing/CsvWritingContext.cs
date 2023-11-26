using System.Reflection;
using CsvMagic.Reflection;
using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public class CsvWritingContext {
    public CsvOptions Options { get; }
    private readonly IReadOnlyDictionary<Type, FieldRenderer> renderers;

    public CsvWritingContext(CsvOptions options, IReadOnlyDictionary<Type, FieldRenderer> renderers) {
        this.renderers = renderers;
        Options = options;
    }

    public FieldRenderer GetRendererFor(PropertyInfo p) {
        var fieldAttr = AttributeHelper.GetCsvFieldAttribute(p);
        FieldRenderer? renderer = null;
        if (fieldAttr != null && fieldAttr.Renderer != null) {
            renderer = (FieldRenderer?)Activator.CreateInstance(fieldAttr.Renderer);
        }

        return renderer ?? GetRendererFor(p.PropertyType) ?? GetDefaultParser(p.PropertyType);
    }

    private static FieldRenderer GetDefaultParser(Type type) {
        var genericType = typeof(ComplexTypeRenderer<>);
        var notGenericType = genericType.MakeGenericType(new[] { type });
        FieldRenderer parser = (FieldRenderer)Activator.CreateInstance(notGenericType);
        return parser;
    }

    private FieldRenderer? GetRendererFor(Type t) {
        return renderers.ContainsKey(t) ? renderers[t] : null;
    }
}
