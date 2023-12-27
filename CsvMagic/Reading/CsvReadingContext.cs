using System.Reflection;
using CsvMagic.Helpers;
using CsvMagic.Reading.Parsers;
using CsvMagic.Reflection;

namespace CsvMagic.Reading;

public class CsvReadingContext {
    public CsvOptions Options { get; }
    private readonly IReadOnlyDictionary<Type, FieldParser> parsers;
    private readonly IDictionary<(Type?, string), FieldParser> fieldParsers;
    private readonly IDictionary<Type, RowFactory> factories;
    private readonly StreamReader streamReader;
    public int LastReadLineNumber { get; private set; } = -1;
    public string? LastReadLine { get; private set; } = null;

    public CsvReadingContext(CsvOptions options, IReadOnlyDictionary<Type, FieldParser> parsers,
        IDictionary<(Type?, string), FieldParser> fieldParsers, IDictionary<Type, RowFactory> factories,
        StreamReader streamReader) {
        this.parsers = parsers;
        this.fieldParsers = fieldParsers;
        this.streamReader = streamReader;
        this.factories = factories;
        Options = options;
    }

    public RowFactory GetFactoryFor(Type type) {
        return factories.GetOrDefault(type, () => {
            var genericType = typeof(EmptyConstructorRowFactory<>);
            var notGenericType = genericType.MakeGenericType(new[] { type });
            RowFactory parser = (RowFactory)Activator.CreateInstance(notGenericType);
            return parser;
        });
    }

    public FieldParser GetParserFor(PropertyInfo p) {
        var key = (p.DeclaringType, p.Name);
        return fieldParsers.GetOrDefault(key, () => GetParserFor(p.PropertyType) ?? GetDefaultParser(p.PropertyType));
    }

    private static FieldParser GetDefaultParser(Type type) {
        var genericType = typeof(ComplexTypeParser<>);
        var notGenericType = genericType.MakeGenericType(new[] { type });
        FieldParser parser = (FieldParser)Activator.CreateInstance(notGenericType);
        return parser;
    }

    private FieldParser? GetParserFor(Type t) {
        return parsers.ContainsKey(t) ? parsers[t] : null;
    }

    public async Task<string?> NextLine() {
        var line = await streamReader.ReadLineAsync();
        LastReadLineNumber++;
        LastReadLine = line;
        return line;
    }

    public bool HasMoreLines() {
        return !streamReader.EndOfStream;
    }
}
