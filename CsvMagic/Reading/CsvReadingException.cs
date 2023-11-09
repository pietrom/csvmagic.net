namespace CsvMagic.Reading;

public class CsvReadingException : Exception
{
    public CsvReadingException(Exception nested) : base("Error reading CSV", nested)
    {
    }

    public required int LineNumber { get; init; }
    public required string? LineText { get; init; }
}