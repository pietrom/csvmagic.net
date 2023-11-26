using System.Linq.Expressions;
using System.Reflection;
using CsvMagic.Writing.Renderers;

namespace CsvMagic.Writing;

public class CsvWritingEngine<TRow> {
    private readonly IReadOnlyDictionary<Type, FieldRenderer> renderers;
    private readonly IDictionary<(Type, string), FieldRenderer> fieldRenderers;
    private readonly IDictionary<(Type, string), string> fieldLabels;
    private readonly ComplexTypeRenderer<TRow> rootRenderer;

    internal CsvWritingEngine(IReadOnlyDictionary<Type, FieldRenderer> renderers) {
        this.renderers = renderers;
        rootRenderer = new ComplexTypeRenderer<TRow>();
        fieldRenderers = new Dictionary<(Type, string), FieldRenderer>();
        fieldLabels = new Dictionary<(Type, string), string>();
    }

    public async Task Write(CsvOptions options, IEnumerable<TRow> rows, StreamWriter writer) {
        var context = new CsvWritingContext(options, renderers, fieldRenderers, fieldLabels);
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

    public ConfigurationBuilder Configure<TField>(Expression<Func<TRow, TField>> cfg) {
        var propertyInfo = ((MemberExpression)cfg.Body).Member;
        return new PrivateConfigurationBuilder(this, propertyInfo);
    }

    public interface ConfigurationBuilder {
        ConfigurationBuilder UsingLabel(string label);
        ConfigurationBuilder UsingRenderer(FieldRenderer renderer);
    }

    private class PrivateConfigurationBuilder : ConfigurationBuilder {
        private readonly CsvWritingEngine<TRow> engine;
        private readonly MemberInfo propertyInfo;

        public PrivateConfigurationBuilder(CsvWritingEngine<TRow> engine, MemberInfo propertyInfo) {
            this.engine = engine;
            this.propertyInfo = propertyInfo;
        }

        public ConfigurationBuilder UsingLabel(string label) {
            this.engine.fieldLabels.Add((propertyInfo.DeclaringType, propertyInfo.Name), label);
            return this;
        }

        public ConfigurationBuilder UsingRenderer(FieldRenderer renderer) {
            this.engine.fieldRenderers.Add((propertyInfo.DeclaringType, propertyInfo.Name), renderer);
            return this;
        }
    }
}
