using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

public class CustomDateOnlyParser : FieldParser
{
    public object? Parse(CsvOptions options, string? text)
    {
        return string.IsNullOrEmpty(text) ? null : DateOnly.ParseExact(text, "yyyyMMdd");
    }
}