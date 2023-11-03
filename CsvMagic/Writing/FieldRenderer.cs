namespace CsvMagic.Writing;

public interface FieldRenderer
{
    string Render(CsvOptions options, object? value);
}