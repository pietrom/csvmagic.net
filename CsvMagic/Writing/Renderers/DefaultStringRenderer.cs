namespace CsvMagic.Writing.Renderers;

public class DefaultStringRenderer : FieldRenderer
{
    public string Render(CsvOptions options, object? value)
    {
        var text = value as string;
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var escaped = text.Replace($"{options.Quoting}", $"{options.Quoting}{options.Quoting}");

        if (text.Contains(options.Delimiter) || text.Contains(options.Quoting))
        {
            return $"{options.Quoting}{escaped}{options.Quoting}";
        }
        return escaped;
    }
}