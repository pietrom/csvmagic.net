using System.Linq.Expressions;
using System.Reflection;
using CsvMagic.Reading.Parsers;

namespace CsvMagic.Reading;

public interface CsvReadingEngine<TRow> {
    IAsyncEnumerable<TRow> ReadFromStream(CsvOptions options, StreamReader reader);

    ConfigurableCsvReadingEngine<TRow> Configure<TField>(Expression<Func<TRow, TField>> cfg);
}

public interface ConfigurableCsvReadingEngine<TRow> : CsvReadingEngine<TRow> {
    ConfigurableCsvReadingEngine<TRow> UsingParser(FieldParser renderer);
    ConfigurableCsvReadingEngine<TRow> UsingFactory<TField>(RowFactoryDelegate<TField> factory);
    ConfigurableCsvReadingEngine<TRow> UsingFactory(Type type, RowFactory factory);
}

internal class SimpleCsvReadingEngine<TRow> : CsvReadingEngine<TRow> {
    private readonly IReadOnlyDictionary<Type, FieldParser> parsers;
    private readonly IDictionary<(Type?, string), FieldParser> fieldParsers;
    private readonly IDictionary<Type, RowFactory> factories;
    private readonly FieldParser rootParser;

    internal SimpleCsvReadingEngine(IReadOnlyDictionary<Type, FieldParser> parsers, RowFactory factory) {
        this.parsers = parsers;
        rootParser = new ComplexTypeParser<TRow>();
        fieldParsers = new Dictionary<(Type?, string), FieldParser>();
        factories = new Dictionary<Type, RowFactory>() {
            { typeof(TRow), factory }
        };
    }

    public async IAsyncEnumerable<TRow> ReadFromStream(CsvOptions options, StreamReader reader) {
        var context = new CsvReadingContext(options, parsers, fieldParsers, factories, reader);

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

    public ConfigurableCsvReadingEngine<TRow> Configure<TField>(Expression<Func<TRow, TField>> cfg) {
        var propertyInfo = ((MemberExpression)cfg.Body).Member;
        return new PrivateConfigurableReadingEngine(this, propertyInfo);
    }

    private class PrivateConfigurableReadingEngine : ConfigurableCsvReadingEngine<TRow> {
        private readonly SimpleCsvReadingEngine<TRow> engine;
        private readonly MemberInfo propertyInfo;

        public PrivateConfigurableReadingEngine(SimpleCsvReadingEngine<TRow> engine, MemberInfo propertyInfo) {
            this.engine = engine;
            this.propertyInfo = propertyInfo;
        }

        public ConfigurableCsvReadingEngine<TRow> UsingParser(FieldParser renderer) {
            this.engine.fieldParsers.Add((propertyInfo.DeclaringType, propertyInfo.Name), renderer);
            return this;
        }

        public ConfigurableCsvReadingEngine<TRow> UsingFactory<TField>(RowFactoryDelegate<TField> factory) {
            return this.UsingFactory(typeof(TField), new RowFactoryDelegateWrapper<TField>(factory));
        }

        public ConfigurableCsvReadingEngine<TRow> UsingFactory(Type type, RowFactory factory) {
            this.engine.factories.Add(type, factory);
            return this;
        }

        public IAsyncEnumerable<TRow> ReadFromStream(CsvOptions options, StreamReader reader) {
            return this.engine.ReadFromStream(options, reader);
        }

        public ConfigurableCsvReadingEngine<TRow> Configure<TField>(Expression<Func<TRow, TField>> cfg) {
            return this.engine.Configure(cfg);
        }
    }
}
