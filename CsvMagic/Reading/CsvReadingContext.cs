using System.Reflection;
using CsvMagic.Reading.Parsers;
using CsvMagic.Reflection;

namespace CsvMagic.Reading;

public class CsvReadingContext {
    public CsvOptions Options { get; }
    private readonly IReadOnlyDictionary<Type, FieldParser> parsers;
    private readonly StreamReader streamReader;
    private static readonly FieldParser DefaultParser = new DefaultParser();
    public int LastReadLineNumber { get; private set; } = -1;
    public string? LastReadLine { get; private set; } = null;

    public CsvReadingContext(CsvOptions options, IReadOnlyDictionary<Type, FieldParser> parsers,
        StreamReader streamReader) {
        this.parsers = parsers;
        this.streamReader = streamReader;
        Options = options;
    }

    public FieldParser GetParserFor(PropertyInfo p) {
        var fieldAttr = AttributeHelper.GetCsvFieldAttribute(p);
        FieldParser? parser = null;
        if (fieldAttr != null && fieldAttr.Parser != null) {
            parser = (FieldParser?)Activator.CreateInstance(fieldAttr.Parser);
        }

        return parser ?? GetParserFor(p.PropertyType) ?? GetDefaultParser(p.PropertyType);
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
