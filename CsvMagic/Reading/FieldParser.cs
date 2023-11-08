namespace CsvMagic.Reading;

public interface FieldParser
{
    (object?, string?) ParseNext(CsvReadingContext context, string? text);
}