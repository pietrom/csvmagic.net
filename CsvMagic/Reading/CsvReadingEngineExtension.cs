using System.Text;

namespace CsvMagic.Reading;

public static class CsvReadingEngineExtension {
    public static IAsyncEnumerable<TRow> ReadFromString<TRow>(this CsvReadingEngine<TRow> engine, CsvOptions options, string text) where TRow : new() {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        return engine.Read(options, new StreamReader(stream));
    }

    public static IAsyncEnumerable<TRow> ReadFromFile<TRow>(this CsvReadingEngine<TRow> engine, CsvOptions options, string filePath) where TRow : new() {
        var stream = File.OpenRead(filePath);
        return engine.Read(options, new StreamReader(stream));
    }
}
