using System.Reflection;
using CsvMagic.Reflection;
using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public class CsvWritingEngine<TRow>
{
    private static readonly FieldRenderer DefaultRenderer = new DefaultRenderer();
    private readonly IReadOnlyList<(PropertyInfo, FieldRenderer)> metadata;
    private readonly CsvOptions? tagOptions;

    internal CsvWritingEngine(IReadOnlyDictionary<Type, FieldRenderer> renderers)
    {
        metadata = InitSerializers(renderers);
        tagOptions = AttributeHelper.GetCsvRowAttribute(typeof(TRow))?.Options;
    }

    private IReadOnlyList<(PropertyInfo, FieldRenderer)> InitSerializers(
        IReadOnlyDictionary<Type, FieldRenderer> serializers)
    {
        return ReflectionHelper.GetTypeProperties(typeof(TRow))
            .Where(p => p.CanRead)
            .Select(
                p => (p, GetSerializerFor(p) ?? (serializers.ContainsKey(p.PropertyType)
                            ? serializers[p.PropertyType]
                            : DefaultRenderer)
                    )
            ).ToList();
    }

    public async Task Write(IEnumerable<TRow> rows, StreamWriter writer, CsvOptions? localOptions = null)
    {
        var actualOptions = localOptions ?? tagOptions ?? throw new System.Exception($"{typeof(TRow).Name} should be annotated with [CsvRow] attribute");
        if (actualOptions.HandleHeaderRow)
        {
            var headers = BuildHeadersRow(actualOptions.Delimiter);
            await writer.WriteLineAsync(headers);
        }

        foreach (var row in rows)
        {
            await writer.WriteLineAsync(BuildDataRow(actualOptions, row));
        }

        await writer.FlushAsync();
    }

    private string BuildDataRow(CsvOptions options, TRow row)
    {
        return string.Join(options.Delimiter, metadata.Select(x => x.Item2.Render(options, x.Item1.GetValue(row))));
    }

    private FieldRenderer? GetSerializerFor(PropertyInfo p)
    {
        var fieldAttr = AttributeHelper.GetCsvFieldAttribute(p);
        FieldRenderer? serializer = null;
        if (fieldAttr != null && fieldAttr.Renderer != null)
        {
            serializer = (FieldRenderer?)Activator.CreateInstance(fieldAttr.Renderer);
        }

        return serializer;
    }

    private string BuildHeadersRow(char delimiter)
    {
        return string.Join(delimiter, metadata.Select(x => x.Item1.Name));
    }
}