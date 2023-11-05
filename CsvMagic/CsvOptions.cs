namespace CsvMagic;

public record CsvOptions(char Delimiter, char Quoting, char DecimalSeparator, bool HandleHeaderRow);