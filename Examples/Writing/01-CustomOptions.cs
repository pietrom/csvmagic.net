using CsvMagic;
using CsvMagic.Writing;

namespace Examples.Writing;

public static class CustomOptions {
    public static Task Run() {
        var engine = new CsvWritingEngineFactory().Create<FizzBuzzData>();
        var options = CsvOptions.Builder()
                .WithHeaders()
                .WithDelimiter(';')
                .WithQuoting('\'')
                .WithDecimalSeparator(',')
                .Build()
            ;
        var people = new[] {
            new FizzBuzzData("fizz", 3, new DateOnly(2023, 12, 3), 3.03m),
            new FizzBuzzData("buzz", 5, new DateOnly(2023, 12, 5), 5.05m),
            new FizzBuzzData("fizzbuzz", 15, new DateOnly(2023, 12, 15), 15.15m),
            new FizzBuzzData("fizzbuzz \"advanced\"", 105, new DateOnly(2023, 12, 15), 105.105m),
        };
        return engine.WriteToFile(options, people, "01-custom-options.csv");
    }

    private record FizzBuzzData(string Text, int IntValue, DateOnly DateValue, decimal DecimalValue);
}
