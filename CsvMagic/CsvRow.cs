namespace CsvMagic;

[AttributeUsage(AttributeTargets.Class)]
public class CsvRow : Attribute
{
    public char Delimiter { get; set; } = ',';
    public char Quoting { get; set; } = '"';
    public char DecimalSeparator { get; set; } = '.';
    public bool HandleHeaderRow { get; set; } = true;

    public CsvOptions Options => new CsvOptions(Delimiter, Quoting, DecimalSeparator, HandleHeaderRow);
}
