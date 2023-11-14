using CsvMagic;
using CsvMagic.Writing;

namespace Samples.WriteCsv;

public static class GettingStartedProgram {
    public static async Task Execute() {
        var engine = new CsvWritingEngineFactory().Create<SampleRow>();

        var rows = new[] {
            new SampleRow {
                Id = 1, FirstName = "Eddy", LastName = "Merckx", BirtdDay = new DateOnly(1947, 6, 17), Ranking = 100
            },
            new SampleRow {
                Id = 2, FirstName = "Bernard \"Le Blaireau\"", LastName = "Hinault",
                BirtdDay = new DateOnly(1954, 11, 14), Ranking = 95.7m
            },
            new SampleRow {
                Id = 100, FirstName = "Miguel", LastName = "Indurain", BirtdDay = new DateOnly(1964, 7, 16),
                Ranking = 75.2m
            },
            new SampleRow {
                Id = 501, FirstName = "Alberto", LastName = "Contador", BirtdDay = new DateOnly(1982, 12, 6),
                Ranking = 81.3m
            }
        };

        var options = CsvOptions.Default();

        // var options = CsvOptions.Builder()
        //     .WithoutHeaders()
        //     .WithDelimiter(';')
        //     .WithQuoting('\'')
        //     .WithDecimalSeparator(',')
        //     .Build();

        await engine.Write(options, rows, new StreamWriter(File.OpenWrite("cyclists.csv")));
    }
}
