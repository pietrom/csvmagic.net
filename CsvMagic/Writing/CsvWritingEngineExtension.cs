using System.Text;

namespace CsvMagic.Writing;

public static class CsvWritingEngineExtension {
    public static async Task<string> WriteToString<T>(this CsvWritingEngine<T> engine, CsvOptions options, IEnumerable<T> items) {
        return Encoding.UTF8.GetString(await engine.WriteToByteArray(options, items));
    }

    public static async Task<byte[]> WriteToByteArray<T>(this CsvWritingEngine<T> engine, CsvOptions options, IEnumerable<T> items) {
        var stream = new MemoryStream();
        await engine.Write(options, items, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        return stream.ToArray();
    }

    public static async Task WriteToFile<T>(this CsvWritingEngine<T> engine, CsvOptions options, IEnumerable<T> items, string filePath) {
        var stream = File.OpenWrite(filePath);
        await engine.Write(options, items, new StreamWriter(stream));
        stream.Close();
    }

    public static Task<string> WriteToString<T>(this CsvWritingEngine<T> engine, IList<T> items) {
        return engine.WriteToString(CsvOptions.Default(), items);
    }
}
