using System.Text;

namespace CsvMagic.Writing;

public static class CsvWritingEngineExtension
{
    public static async Task<string> Write<T>(this CsvWritingEngine<T> engine, IList<T> items)
    {
        var stream = new MemoryStream();
        await engine.Write(items, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}