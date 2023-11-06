using System.Text;

namespace CsvMagic.Writing;

public static class CsvWritingEngineExtension
{
    public static async Task<string> Write<T>(this CsvWritingEngine<T> engine, IList<T> items, CsvOptions options)
    {
        var stream = new MemoryStream();
        await engine.Write(items, new StreamWriter(stream),  options);
        stream.Seek(0, SeekOrigin.Begin);
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public static Task<string> Write<T>(this CsvWritingEngine<T> engine, IList<T> items)
    {
        return engine.Write(items, CsvOptions.Default());
    }
}