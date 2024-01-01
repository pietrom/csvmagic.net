using CsvMagic;
using CsvMagic.Reading;
using CsvMagic.Reading.Parsers;

namespace Examples.Reading;

public static class CustomParserPerProperty {
    public static async Task Run() {
        Console.WriteLine("05-CustomParserPerProperty");
        var engine = new CsvReadingEngineFactory()
            .Create<CsvRow>()
            .Configure(x => x.SecondValue).UsingParser(new CustomDecimalParser(100))
            .Configure(x => x.ThirdValue).UsingParser(new CustomDecimalParser(1000));
        var options = CsvOptions.Default();
        var data = await engine.ReadFromFile(options, "Reading/05-custom-parser-per-property.input.csv").ToListAsync();
        foreach (var item in data) {
            Console.WriteLine(item);
        }

        Console.WriteLine();
    }

    record CsvRow {
        public decimal FirstValue { get; set; }
        public decimal SecondValue { get; set; }
        public decimal ThirdValue { get; set; }
    }

    class CustomDecimalParser : QuotingParser<decimal> {
        private readonly int factor;
        public CustomDecimalParser(int factor) {
            this.factor = factor;
        }

        protected override decimal ParseValue(CsvReadingContext context, string? value) {
            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentException("Illegal decimal value");
            }

            var parsedValue = decimal.Parse(value);
            return parsedValue / this.factor;
        }
    }
}
