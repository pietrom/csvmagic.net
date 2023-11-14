using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public class CsvWritingEngine<TRow> {
    private readonly IReadOnlyDictionary<Type, FieldRenderer> renderers;
    private readonly ComplexTypeRenderer<TRow> rootRenderer;

    internal CsvWritingEngine(IReadOnlyDictionary<Type, FieldRenderer> renderers) {
        this.renderers = renderers;
        rootRenderer = new ComplexTypeRenderer<TRow>();
    }

    public async Task Write(CsvOptions options, IEnumerable<TRow> rows, StreamWriter writer) {
        var context = new CsvWritingContext(options, renderers);
        if (options.HandleHeaderRow) {
            var headers = rootRenderer.RenderHeader(context);
            await writer.WriteLineAsync(headers);
        }

        foreach (var row in rows) {
            var rowText = rootRenderer.RenderObject(context, row);
            await writer.WriteLineAsync(rowText);
        }

        await writer.FlushAsync();
    }
}
