namespace CsvMagic.Reading.Parsers;

public class DefaultParser : SimpleParser<string> {
    protected override string ParseValue(CsvReadingContext context, string value) {
        return value;
    }
}
