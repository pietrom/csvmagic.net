using System.Globalization;

namespace CsvMagic.Reading.Parsers;

public class DefaultDateTimeOffsetParser : QuotingParser<DateTimeOffset?>
{
    protected override DateTimeOffset? ParseValue(CsvOptions options, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return DateTimeOffset.ParseExact(value, "O", null, DateTimeStyles.None);
    }
}