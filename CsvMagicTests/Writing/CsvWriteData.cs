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

public record CsvWriteDataWithCustomLabels : Data {
    [CsvField(Label = "TextValue")]
    public string? StringValue { get; set; }
    public long LongValue { get; set; }
    [CsvField(Renderer = typeof(DateOnlyRenderer))]
    public DateOnly BirthDay { get; set; }
    [CsvField(Label = "Another Day")]
    public DateOnly OtherDay { get; set; }
    [CsvField(Label = "Another String")]
    public string? OtherString { get; set; }
}
