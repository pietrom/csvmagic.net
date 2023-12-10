namespace CsvMagic.Reading.Parsers;

public class DefaultUintParser : SimpleParser<uint> {
    protected override uint ParseValue(CsvReadingContext context, string value) {
        return uint.Parse(value);
    }
}
