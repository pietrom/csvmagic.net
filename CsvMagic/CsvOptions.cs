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

internal record SimpleCsvOptions(char Delimiter, char Quoting, char DecimalSeparator, bool HandleHeaderRow, bool FullyQualifyNestedProperties) : CsvOptions {
    public static CsvOptionsBuilder Builder() => new CsvOptionsBuilder();
}

public class CsvOptionsBuilder {
    private char delimiter = ',';
    private char quoting = '"';
    private char decimalSeparator = '.';
    private bool handleHeaderRow = true;
    private bool fullyQualifyNestedProperties = false;

    public CsvOptionsBuilder WithDelimiter(char delimiter) {
        this.delimiter = delimiter;
        return this;
    }

    public CsvOptionsBuilder WithQuoting(char quoting) {
        this.quoting = quoting;
        return this;
    }

    public CsvOptionsBuilder WithDecimalSeparator(char decimalSeparator) {
        this.decimalSeparator = decimalSeparator;
        return this;
    }

    public CsvOptionsBuilder WithoutHeaders() {
        handleHeaderRow = false;
        return this;
    }

    public CsvOptionsBuilder WithHeaders() {
        handleHeaderRow = true;
        return this;
    }

    public CsvOptionsBuilder FullyQualifyNestedProperties() {
        fullyQualifyNestedProperties = true;
        return this;
    }

    public CsvOptions Build() => new SimpleCsvOptions(delimiter, quoting, decimalSeparator, handleHeaderRow, fullyQualifyNestedProperties);
}
