namespace CsvMagic;

public record CsvOptions(char Delimiter, char Quoting, char DecimalSeparator, bool HandleHeaderRow) {
    public static CsvOptions Default() => new CsvOptionsBuilder().Build();

    public static CsvOptionsBuilder Builder() => new CsvOptionsBuilder();
}

public class CsvOptionsBuilder {
    private char delimiter = ',';
    private char quoting = '"';
    private char decimalSeparator = '.';
    private bool handleHeaderRow = true;

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

    public CsvOptions Build() => new CsvOptions(delimiter, quoting, decimalSeparator, handleHeaderRow);
}
