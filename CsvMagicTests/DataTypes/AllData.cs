namespace CsvMagicTests.DataTypes;

public record AllData {
    public string Text { get; set; }
    public int IntValue { get; set; }
    public long LongValue { get; set; }
    public decimal DecimalValue { get; set; }
    public double DoubleValue { get; set; }
    public short ShortValue { get; set; }
    public uint UintValue { get; set; }
    public float FloatValue { get; set; }
    public bool BoolValue { get; set; }
    public DateOnly DateOnlyValue { get; set; }
    public DateTimeOffset DateTimeOffsetValue { get; set; }
}
