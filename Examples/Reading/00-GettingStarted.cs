using CsvMagic;
using CsvMagic.Reading;

namespace Examples.Reading;

public static class GettingStarted {
    public static async Task Run() {
        Console.WriteLine("00-GettingSTarted");
        var engine = new CsvReadingEngineFactory().Create<CsvData>();
        var options = CsvOptions.Default();
        var data = await engine.ReadFromFile(options, "Reading/00-getting-started.input.csv").ToListAsync();
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
