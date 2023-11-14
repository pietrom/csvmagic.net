using CsvMagic;

namespace CsvMagicTests.Writing;

public record CsvWriteData : Data {
    public string? StringValue { get; set; }
    public long LongValue { get; set; }
    [CsvField(Renderer = typeof(DateOnlyRenderer))]
    public DateOnly BirthDay { get; set; }
    public DateOnly OtherDay { get; set; }
    public string? OtherString { get; set; }
}
