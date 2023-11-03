namespace CsvMagic.Reading;

public interface FieldParser
{
    object? Parse(CsvOptions options, string? text);
}