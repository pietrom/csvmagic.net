using CsvMagic;
using CsvMagic.Reading;

namespace Samples.ReadCsv;

public static class SetOnlyPropertyProgram {
    public static async Task Execute() {
        var engine = new CsvReadingEngineFactory().Create<SampleRowContainingSetOnlyProperty>();

        await File.WriteAllTextAsync("cyclists.csv", @"Id,FullName
19,Pietro Martinelli
");

        var row = await engine.Read(CsvOptions.Default(), new StreamReader(File.OpenRead("cyclists.csv"))).SingleAsync();
        Console.WriteLine(row);
    }
}