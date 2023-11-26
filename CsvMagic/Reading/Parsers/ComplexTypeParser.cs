using System.Reflection;
using CsvMagic.Reflection;

namespace CsvMagic.Reading.Parsers;

public class ComplexTypeParser<TRow> : FieldParser {
    private IReadOnlyList<(PropertyInfo, FieldParser)>? metadata;

    public (object?, string?) ParseNext(CsvReadingContext context, string? text) {
        metadata ??= InitParsers(context);

        var fields = new Dictionary<string, object?>();
        var rest = text;
        foreach (var (info, parser) in metadata) {
            if (rest == null) {
                throw new CsvReadingException(context) {
                    ParserTag = nameof(ComplexTypeParser<TRow>),
                    TokenText = rest,
                    ErrorDetail = "Less Tokens Than Properties"
                };
            }

            (var value, rest) = parser.ParseNext(context, rest);
            fields.Add(info.Name, value);
        }

        var factory = context.GetFactoryFor(typeof(TRow));
        var row = factory.Create(fields);
        return (row, rest);
    }

    private IReadOnlyList<(PropertyInfo, FieldParser)> InitParsers(CsvReadingContext context) {
        return ReflectionHelper.GetTypeProperties(typeof(TRow))
            .Where(p => p.CanWrite)
            .Select(
                p => (p, context.GetParserFor(p)
                    )
            ).ToList();
    }
}
