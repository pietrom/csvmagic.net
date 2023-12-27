using System.Linq.Expressions;
using System.Reflection;
using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public interface CsvWritingEngine<TRow> {
    Task WriteToStream(CsvOptions options, IEnumerable<TRow> rows, StreamWriter writer);

    ConfigurableCsvWritingEngine<TRow> Configure<TField>(Expression<Func<TRow, TField>> cfg);
}

public interface ConfigurableCsvWritingEngine<TRow> : CsvWritingEngine<TRow> {
    ConfigurableCsvWritingEngine<TRow> UsingLabel(string label);
    ConfigurableCsvWritingEngine<TRow> UsingRenderer(FieldRenderer renderer);
}

internal class SimpleCsvWritingEngine<TRow> : CsvWritingEngine<TRow> {
    private readonly IReadOnlyDictionary<Type, FieldRenderer> renderers;
    private readonly FieldLabelWritingStrategy fieldLabelWritingStrategy;
    private readonly IDictionary<(Type, string), FieldRenderer> fieldRenderers;
    private readonly IDictionary<(Type, string), string> fieldLabels;
    private readonly ComplexTypeRenderer<TRow> rootRenderer;

    internal SimpleCsvWritingEngine(IReadOnlyDictionary<Type, FieldRenderer> renderers) : this(renderers, new DefaultFieldLabelWritingStrategy()) { }

    internal SimpleCsvWritingEngine(IReadOnlyDictionary<Type, FieldRenderer> renderers, FieldLabelWritingStrategy fieldLabelWritingStrategy) {
        this.renderers = renderers;
        this.fieldLabelWritingStrategy = fieldLabelWritingStrategy;
        rootRenderer = new ComplexTypeRenderer<TRow>();
        fieldRenderers = new Dictionary<(Type, string), FieldRenderer>();
        fieldLabels = new Dictionary<(Type, string), string>();
    }

    public async Task WriteToStream(CsvOptions options, IEnumerable<TRow> rows, StreamWriter writer) {
        var context = new CsvWritingContext(options, renderers, fieldRenderers, fieldLabels, fieldLabelWritingStrategy);
        if (options.HandleHeaderRow) {
            var headers = rootRenderer.RenderHeader(context);
            await writer.WriteLineAsync(string.Join(options.Delimiter, headers));
        }

        foreach (var row in rows) {
            var rowText = rootRenderer.RenderObject(context, row);
            await writer.WriteLineAsync(rowText);
        }

        await writer.FlushAsync();
    }

    public ConfigurableCsvWritingEngine<TRow> Configure<TField>(Expression<Func<TRow, TField>> cfg) {
        var propertyInfo = ((MemberExpression)cfg.Body).Member;
        return new PrivateConfigurableWritingEngine(this, propertyInfo);
    }

    private class PrivateConfigurableWritingEngine : ConfigurableCsvWritingEngine<TRow> {
        private readonly SimpleCsvWritingEngine<TRow> engine;
        private readonly MemberInfo propertyInfo;

        public PrivateConfigurableWritingEngine(SimpleCsvWritingEngine<TRow> engine, MemberInfo propertyInfo) {
            this.engine = engine;
            this.propertyInfo = propertyInfo;
        }

        public ConfigurableCsvWritingEngine<TRow> UsingLabel(string label) {
            this.engine.fieldLabels.Add((propertyInfo.DeclaringType, propertyInfo.Name), label);
            return this;
        }

        public ConfigurableCsvWritingEngine<TRow> UsingRenderer(FieldRenderer renderer) {
            this.engine.fieldRenderers.Add((propertyInfo.DeclaringType, propertyInfo.Name), renderer);
            return this;
        }

        public Task WriteToStream(CsvOptions options, IEnumerable<TRow> rows, StreamWriter writer) {
            return this.engine.WriteToStream(options, rows, writer);
        }

        public ConfigurableCsvWritingEngine<TRow> Configure<TField>(Expression<Func<TRow, TField>> cfg) {
            return this.engine.Configure(cfg);
        }
    }
}
