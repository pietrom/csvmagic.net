using CsvMagic;
using CsvMagic.Reading;

namespace Samples.ReadCsv;

public static class GettingStartedProgram {
    public static async Task Execute() {
        var engine = new CsvReadingEngineFactory().Create<SampleRow>();

        await File.WriteAllTextAsync("cyclists.csv", @"Id,FirstName,LastName,BirtdDay,Ranking,FullName
2,""Bernard """"Le Blaireau"""""",Hinault,1954-11-14,95.7
100,Miguel,Indurain,1964-07-16,75.2
");

        var rows = await engine.Read(CsvOptions.Default(), new StreamReader(File.OpenRead("cyclists.csv"))).ToListAsync();

        foreach (var row in rows) {
            Console.WriteLine(row);
        }
    }
}
