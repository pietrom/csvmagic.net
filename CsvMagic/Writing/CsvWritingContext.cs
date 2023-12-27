using System.Reflection;
using CsvMagic.Helpers;
using CsvMagic.Reflection;
using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public class CsvWritingContext {
    public CsvOptions Options { get; }
    private readonly IReadOnlyDictionary<Type, FieldRenderer> renderers;
    private readonly IDictionary<(Type?, string), FieldRenderer> fieldRenderers;
    private readonly IDictionary<(Type?, string), string> fieldLabels;
    private readonly FieldLabelWritingStrategy fieldLabelWritingStrategy;

    public CsvWritingContext(CsvOptions options, IReadOnlyDictionary<Type, FieldRenderer> renderers,
        IDictionary<(Type?, string), FieldRenderer> fieldRenderers, IDictionary<(Type?, string), string> fieldLabels,
        FieldLabelWritingStrategy fieldLabelWritingStrategy) {
        this.renderers = renderers;
        this.fieldRenderers = fieldRenderers;
        this.fieldLabels = fieldLabels;
        this.fieldLabelWritingStrategy = fieldLabelWritingStrategy;
        Options = options;
    }

    public string GetLabelFor(PropertyInfo info) {
        (Type?, string) key = (info.DeclaringType, info.Name);
        return fieldLabels.GetOrDefault(key, () => fieldLabelWritingStrategy.GetLabel(info));
    }

    public FieldRenderer GetRendererFor(PropertyInfo p) {
        (Type?, string) key = (p.DeclaringType, p.Name);
        return fieldRenderers.GetOrDefault(key, () => GetRendererFor(p.PropertyType) ?? GetDefaultParser(p.PropertyType));
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
