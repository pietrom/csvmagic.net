namespace CsvMagic.Reading;

public class CsvReadingException : Exception
{
    public CsvReadingException(Exception nested) : base("Error reading CSV", nested)
    {
    }

    public required int ErrorLineNumber { get; init; }
    public required string? ErrorLineText { get; init; }
}