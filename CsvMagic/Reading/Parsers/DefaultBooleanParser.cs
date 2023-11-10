using System.Globalization;

namespace CsvMagic.Reading.Parsers;

public class DefaultBooleanParser : QuotingParser<bool?> {
    private readonly string trueText;
    private readonly string falseText;

    public DefaultBooleanParser(string trueText, string falseText) {
        this.trueText = trueText;
        this.falseText = falseText;
    }

    public DefaultBooleanParser() : this("1", "0") { }

    protected override bool? ParseValue(CsvReadingContext context, string? value) {
        if (string.IsNullOrEmpty(value)) {
            return null;
        }

        if (value == trueText) {
            return true;
        }

        if (value == falseText) {
            return false;
        }

        throw new CsvReadingException(context) {
            TokenText = value,
            ParserTag = nameof(DefaultBooleanParser),
            ErrorDetail =
                $"Unknown value: '{value}'. Known values are '{trueText}' (for true) and '{falseText}' (for false)"
        };
    }
}