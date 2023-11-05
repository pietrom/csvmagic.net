using CsvMagic;
using CsvMagic.Reading;
using CsvMagic.Reading.Parsers;

namespace CsvMagicTests.Reading;

public class CustomDateOnlyParser : SimpleParser<DateOnly>
{
    protected override DateOnly ParseValue(CsvOptions options, string value)
    {
        return DateOnly.ParseExact(value, "yyyyMMdd");
    }
}