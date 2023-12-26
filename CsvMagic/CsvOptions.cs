namespace CsvMagic;

public interface CsvOptions {
    char Delimiter { get; }
    char Quoting { get; }
    char DecimalSeparator { get; }
    bool HandleHeaderRow { get; }
    bool FullyQualifyNestedProperties { get; }

    public static CsvOptions Default() => new CsvOptionsBuilder().Build();

    public static CsvOptionsBuilder Builder() => new CsvOptionsBuilder();
}
