using System.Globalization;

namespace CsvMagic.Reading.Parsers;

public class DefaultDateTimeOffsetParser : SimpleParser<DateTimeOffset>
{
    protected override DateTimeOffset ParseValue(CsvOptions options, string value)
    {
        return DateTimeOffset.ParseExact(value, "O", null, DateTimeStyles.None);
    }
}