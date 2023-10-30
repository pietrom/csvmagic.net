namespace CsvMagic;

[AttributeUsage(AttributeTargets.Class)]
public class CsvRow : Attribute
{
    public char Delimiter { get; set; } = ',';
    public bool HandleHeaderRow { get; set; } = true;
}
