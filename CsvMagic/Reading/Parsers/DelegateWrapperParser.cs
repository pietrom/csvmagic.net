namespace CsvMagic.Reading.Parsers;

public class DelegateWrapperParser : FieldParser
{
    private readonly Func<string?, object?> _delegate;

    public DelegateWrapperParser(Func<string?, object?> @delegate)
    {
        _delegate = @delegate;
    }

    public object? Parse(CsvOptions options, string? text)
    {
        return text == null ? null : _delegate(text);
    }

    public static implicit operator DelegateWrapperParser(Func<string?, object?> f) => new DelegateWrapperParser(f);
}