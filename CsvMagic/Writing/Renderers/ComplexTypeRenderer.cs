﻿using System.Reflection;
using CsvMagic.Reading;
using CsvMagic.Reflection;

namespace CsvMagic.Writing.Renderers;

public class ComplexTypeRenderer<TRow> : FieldRenderer {
    private IReadOnlyList<(PropertyInfo, FieldRenderer)>? metadata;

    public string RenderObject(CsvWritingContext context, object? value) {
        metadata ??= InitSerializers(context);
        return string.Join(context.Options.Delimiter, metadata.Select(x => x.Item2.RenderObject(context, x.Item1.GetValue(value))));
    }

    public IEnumerable<string> RenderHeader(CsvWritingContext context, PropertyInfo? propertyInfo = null) {
        metadata ??= InitSerializers(context);
        var prefix = propertyInfo == null || !context.Options.FullyQualifyNestedProperties ? string.Empty : $"{context.GetLabelFor(propertyInfo)}_";
        return metadata.SelectMany(x => x.Item2.RenderHeader(context, x.Item1).Select(y => $"{prefix}{y}"));
    }

    private IReadOnlyList<(PropertyInfo, FieldRenderer)> InitSerializers(
        CsvWritingContext context) {
        return ReflectionHelper.GetTypeProperties(typeof(TRow))
            .Where(p => p.CanRead)
            .Select(
                p => (p, context.GetRendererFor(p)
                    )
            ).ToList();
    }
}
