namespace CsvMagic.Reading;

public interface FieldParser
{
    (object?, string?) ParseNext(CsvOptions options, string? text);
}