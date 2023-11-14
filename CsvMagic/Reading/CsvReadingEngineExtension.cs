﻿using System.Text;

namespace CsvMagic.Reading;

public static class CsvReadingEngineExtension {
    public static async Task<IEnumerable<TRow>> Read<TRow>(this CsvReadingEngine<TRow> engine, string text, CsvOptions? options = null) where TRow : new() {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        return await engine.Read(options, new StreamReader(stream)).ToListAsync();
    }
}
