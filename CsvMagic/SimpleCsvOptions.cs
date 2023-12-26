namespace CsvMagic;

internal record SimpleCsvOptions(char Delimiter, char Quoting, char DecimalSeparator, bool HandleHeaderRow, bool FullyQualifyNestedProperties) : CsvOptions;
