using System.Reflection;
using CsvMagic.Reflection;

namespace CsvMagic.Reading.Parsers;

public class ComplexTypeParserNew<TRow> : FieldParser where TRow : new()
{
    private IReadOnlyList<(PropertyInfo, FieldParser)>? metadata;

    public (object?, string?) ParseNext(CsvReadingContext context, string? text)
    {
        metadata ??= InitParsers(context);

        var row = new TRow();
        var rest = text;
        foreach (var (info, parser) in metadata)
        {
            (var value, rest) = parser.ParseNext(context, rest);
            info.SetValue(row, value);
        }

        return (row, rest);
    }

    private IReadOnlyList<(PropertyInfo, FieldParser)> InitParsers(CsvReadingContext context)
    {
        return ReflectionHelper.GetTypeProperties(typeof(TRow))
            .Where(p => p.CanWrite)
            .Select(
                p => (p, context.GetParserFor(p)
                    )
            ).ToList();
    }
}