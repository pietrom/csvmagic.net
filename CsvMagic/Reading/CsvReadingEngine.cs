using System.Linq.Expressions;
using System.Reflection;
using CsvMagic.Reading.Parsers;

namespace CsvMagic.Reading;

public class CsvReadingEngine<TRow> where TRow : new() {
    private readonly IReadOnlyDictionary<Type, FieldParser> parsers;
    private readonly IDictionary<(Type?, string), FieldParser> fieldParsers;
    private readonly FieldParser rootParser;

    internal CsvReadingEngine(IReadOnlyDictionary<Type, FieldParser> parsers) {
        this.parsers = parsers;
        rootParser = new ComplexTypeParser<TRow>();
        fieldParsers = new Dictionary<(Type?, string), FieldParser>();
    }

    public async IAsyncEnumerable<TRow> Read(CsvOptions options, StreamReader reader) {
        var context = new CsvReadingContext(options, parsers, fieldParsers, reader);

        if (options.HandleHeaderRow) {
            await context.NextLine();
        }

        while (context.HasMoreLines()) {
            var line = await context.NextLine();
            var rest = line;

            object? row;
            try {
                (row, rest) = rootParser.ParseNext(context, rest);
            } catch (CsvReadingException ex) {
                throw;
            } catch (Exception ex) {
                throw new CsvReadingException(ex, context) {
                    ParserTag = rootParser.GetType().Name,
                    TokenText = rest,
                };
            }

            if (!string.IsNullOrEmpty(rest)) {
                throw new CsvReadingException(context) {
                    ParserTag = nameof(CsvReadingEngine<TRow>),
                    TokenText = rest,
                    ErrorDetail = "Rest Not Empty"
                };
            }

            yield return (TRow)row;
        }
    }

    public ConfigurationBuilder Configure<TField>(Expression<Func<TRow, TField>> cfg) {
        var propertyInfo = ((MemberExpression)cfg.Body).Member;
        return new PrivateConfigurationBuilder(this, propertyInfo);
    }

    public interface ConfigurationBuilder {
        ConfigurationBuilder UsingParser(FieldParser renderer);
    }

    private class PrivateConfigurationBuilder : ConfigurationBuilder {
        private readonly CsvReadingEngine<TRow> engine;
        private readonly MemberInfo propertyInfo;

        public PrivateConfigurationBuilder(CsvReadingEngine<TRow> engine, MemberInfo propertyInfo) {
            this.engine = engine;
            this.propertyInfo = propertyInfo;
        }

        public ConfigurationBuilder UsingParser(FieldParser renderer) {
            this.engine.fieldParsers.Add((propertyInfo.DeclaringType, propertyInfo.Name), renderer);
            return this;
        }
    }
}
