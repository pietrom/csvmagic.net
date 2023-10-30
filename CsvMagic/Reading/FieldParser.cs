namespace CsvMagic.Reading;

public interface FieldParser
{
    object? Parse(string? text);
}