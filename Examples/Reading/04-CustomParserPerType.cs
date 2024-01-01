using CsvMagic;
using CsvMagic.Reading;
using CsvMagic.Reading.Parsers;

namespace Examples.Reading;

public static class CustomParserPerType {
    public static async Task Run() {
        Console.WriteLine("04-CustomParserPerType");
        var engine = new CsvReadingEngineFactory()
            .RegisterParser<decimal>(new CustomDecimalParser())
            .Create<CsvRow>();
        var options = CsvOptions.Default();
        var data = await engine.ReadFromFile(options, "Reading/04-custom-parser-per-type.input.csv").ToListAsync();
        foreach (var item in data) {
            Console.WriteLine(item);
        }
        Console.WriteLine();
    }

    record CsvRow {
        public decimal FirstValue { get; set; }
        public decimal SecondValue { get; set; }
    }

    class CustomDecimalParser : QuotingParser<decimal> {
        protected override decimal ParseValue(CsvReadingContext context, string? value) {
            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentException("Illegal decimal value");
            }

            var parsedValue = decimal.Parse(value);
            return parsedValue / 100;
        }
    }
}
