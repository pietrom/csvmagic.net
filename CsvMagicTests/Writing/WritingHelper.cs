using System.Text;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

public static class WritingHelper
{
    public static async Task<string> Write<T>(this CsvWritingEngine<T> engine, IList<T> items)
    {
        var stream = new MemoryStream();
        await engine.Write(items, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}