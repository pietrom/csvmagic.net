using CsvMagic;

namespace CsvMagicTests.Reading;

public record CsvReadData : Data {
    public string? StringValue { get; set; }
    public long LongValue { get; set; }
    public DateOnly DefaultDateOnly { get; set; }
    [CsvField(Parser = typeof(CustomDateOnlyParser))]
    public DateOnly CustomDateOnly { get; set; }
    public DateOnly? DefaultNullableDateOnly { get; set; }
}
