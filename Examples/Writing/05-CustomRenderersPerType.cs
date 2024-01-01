using CsvMagic;
using CsvMagic.Writing;
using CsvMagic.Writing.Renderers;

namespace Examples.Writing;

public static class CustomRenderersPerType {
    public static Task Run() {
        var engine = new CsvWritingEngineFactory()
            .RegisterRenderer<DateOnly>(new DateOnlyRenderer("yyyyMMdd"))
            .Create<FizzBuzzData>();
        var options = CsvOptions.Default();
        var people = new[] {
            new FizzBuzzData("fizz", 3, new DateOnly(2023, 12, 3), 3.03m),
            new FizzBuzzData("buzz", 5, new DateOnly(2023, 12, 5), 5.05m),
            new FizzBuzzData("fizzbuzz", 15, new DateOnly(2023, 12, 15), 15.15m),
            new FizzBuzzData("fizzbuzz \"advanced\"", 105, new DateOnly(2023, 12, 15), 105.105m),
        };
        return engine.WriteToFile(options, people, "05-custom-renderers-per-type.csv");
    }

    private record FizzBuzzData(string Text, int IntValue, DateOnly DateValue, decimal DecimalValue);

    public class DateOnlyRenderer : QuotableFieldRenderer<DateOnly> {
        private readonly string _format;

        public DateOnlyRenderer(string format = "yyyy-MM-dd") {
            _format = format;
        }

        public DateOnlyRenderer() : this("yyyy-MM-dd") {
        }

        protected override string RenderValue(CsvWritingContext context, DateOnly value) {
            return value.ToString(_format);
        }
    }
}
