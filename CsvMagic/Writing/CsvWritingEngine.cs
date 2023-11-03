using System.Reflection;
using CsvMagic.Reflection;
using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public class CsvWritingEngine<TRow>
{
    private static readonly FieldRenderer DefaultRenderer = new DefaultRenderer();
    private readonly IReadOnlyList<(PropertyInfo, FieldRenderer)> _metadata;
    private readonly CsvRow _csvRowAttr;

    internal CsvWritingEngine(IReadOnlyDictionary<Type, FieldRenderer> renderers)
    {
        _metadata = InitSerializers(renderers);
        _csvRowAttr = AttributeHelper.GetCsvRowAttribute(typeof(TRow)) ?? throw new System.Exception($"{typeof(TRow).Name} should be annotated with [CsvRow] attribute");
    }

    private IReadOnlyList<(PropertyInfo, FieldRenderer)> InitSerializers(
        IReadOnlyDictionary<Type, FieldRenderer> serializers)
    {
        return ReflectionHelper.GetTypeProperties(typeof(TRow))
            .Select(
                p => (p, GetSerializerFor(p) ?? (serializers.ContainsKey(p.PropertyType)
                            ? serializers[p.PropertyType]
                            : DefaultRenderer)
                    )
            ).ToList();
    }

    public async Task Write(IEnumerable<TRow> rows, StreamWriter writer)
    {
        if (_csvRowAttr.HandleHeaderRow)
        {
            var headers = BuildHeadersRow(_csvRowAttr.Delimiter);
            await writer.WriteLineAsync(headers);
        }

        foreach (var row in rows)
        {
            await writer.WriteLineAsync(BuildDataRow(row));
        }

        await writer.FlushAsync();
    }

    private string BuildDataRow(TRow row)
    {
        return string.Join(_csvRowAttr.Delimiter, _metadata.Select(x => Sanitize(x.Item2.Render(x.Item1.GetValue(row)))));
    }

    private string Sanitize(string text)
    {
        var escaped = text.Replace("\"", "\"\"");
        return (text.Contains(_csvRowAttr.Delimiter) || text.Contains('"')) ? $"\"{escaped}\"" : escaped;
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
        return string.Join(delimiter, _metadata.Select(x => x.Item1.Name));
    }
}