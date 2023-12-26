namespace CsvMagic.Reading.Parsers;

public class DefaultUintParser : QuotingParser<uint> {
    protected override uint ParseValue(CsvReadingContext context, string value) {
        return uint.Parse(value);
    }
}
