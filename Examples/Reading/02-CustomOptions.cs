using CsvMagic;
using CsvMagic.Reading;

namespace Examples.Reading;

public static class CustomOptions {
    public static async Task Run() {
        Console.WriteLine("02-CustomOptions");
        var engine = new CsvReadingEngineFactory().Create<CsvData>();
        var options = CsvOptions.Builder()
            .WithDelimiter(';')
            .WithDecimalSeparator(',')
            .WithoutHeaders()
            .Build()
            ;
        var data = await engine.ReadFromFile(options, "Reading/02-custom-options.input.csv").ToListAsync();
        foreach (var item in data) {
            Console.WriteLine(item);
        }
        Console.WriteLine();
    }

    private record CsvData {
        public string Text { get; set; }
        public int IntValue { get; set; }
        public DateOnly When { get; set; }
        public decimal DecimalValue { get; set; }
    }
}
