namespace CsvMagic.Writing.Renderers;

public abstract class QuotableFieldRenderer<T> : FieldRenderer
{
    public string Render(CsvOptions options, object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var text = RenderValue(options, (T)value);

        var escaped = text.Replace($"{options.Quoting}", $"{options.Quoting}{options.Quoting}");

        if (text.Contains(options.Delimiter) || text.Contains(options.Quoting))
        {
            return $"{options.Quoting}{escaped}{options.Quoting}";
        }
        return escaped;
    }

    protected abstract string RenderValue(CsvOptions options, T? value);
}