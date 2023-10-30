namespace CsvMagic.Writing;

public interface FieldRenderer
{
    string Render(object? value);
}