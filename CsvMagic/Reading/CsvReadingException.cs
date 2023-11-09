namespace CsvMagic.Reading;

public class CsvReadingException : Exception
{
    public CsvReadingException(Exception nested, CsvReadingContext context) : base("Error reading CSV", nested)
    {
        LineNumber = context.LastReadLineNumber;
        LineText = context.LastReadLine;
    }

    public CsvReadingException(CsvReadingContext context) : base("Error reading CSV")
    {
        LineNumber = context.LastReadLineNumber;
        LineText = context.LastReadLine;
    }

    public int LineNumber { get; }
    public string? LineText { get; }
    public required string? TokenText { get; init; }
    public required string ParserTag { get; init; }
    public string ErrorDetail { get; init; } = string.Empty;
}