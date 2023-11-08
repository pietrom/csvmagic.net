namespace CsvMagic.Reading.Parsers;

public class DelegateWrapperParser : FieldParser
{
    private readonly Func<string?, (object?, string?)> _delegate;

    public DelegateWrapperParser(Func<string?, (object?, string?)> @delegate)
    {
        _delegate = @delegate;
    }

    public (object?, string?) ParseNext(CsvReadingContext context, string? text)
    {
        return text == null ? (null, null) : _delegate(text);
    }

    public static implicit operator DelegateWrapperParser(Func<string?, (object?, string?)> f) => new DelegateWrapperParser(f);
}