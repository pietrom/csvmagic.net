﻿using System.Reflection;

namespace CsvMagic.Writing.Renderers;

public abstract class QuotableFieldRenderer<T> : FieldRenderer {
    public string RenderObject(CsvWritingContext context, object? value) {
        var text = RenderValue(context, (T)value);

        var escaped = text.Replace($"{context.Options.Quoting}", $"{context.Options.Quoting}{context.Options.Quoting}");

        if (text.Contains(context.Options.Delimiter) || text.Contains(context.Options.Quoting)) {
            return $"{context.Options.Quoting}{escaped}{context.Options.Quoting}";
        }
        return escaped;
    }

    protected abstract string RenderValue(CsvWritingContext context, T? value);


    public IEnumerable<string> RenderHeader(CsvWritingContext context, PropertyInfo? propertyInfo = null) {
        return new[] { propertyInfo != null ? context.GetLabelFor(propertyInfo) : string.Empty };
    }
}
