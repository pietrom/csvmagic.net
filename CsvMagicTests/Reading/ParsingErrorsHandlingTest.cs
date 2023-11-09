using CsvMagic;
using CsvMagic.Reading;
using CsvMagic.Reading.Parsers;
using CsvMagicTests.DataTypes;

namespace CsvMagicTests.Reading;

public class ParsingErrorsHandlingTest
{
    class Row
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    [Test]
    public Task ReadWithoutErrors()
    {
        return Read<Row>(@"Text,Value
A,1
B,2,C,33333
");
    }

    [Test]
    public void ShouldTrackErrorLineNumber()
    {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.LineNumber, Is.EqualTo(3));
    }

    [Test]
    public void ShouldTrackErrorLineText()
    {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.LineText, Is.EqualTo("C,33xx333"));
    }

    [Test]
    public void ShouldTrackTokenText()
    {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.TokenText, Is.EqualTo("33xx333"));
    }

    [Test]
    public void ShouldTrackParserTag()
    {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.ParserTag, Is.EqualTo(nameof(DefaultIntParser)));
    }

    [Test]
    public void ShouldTrackErrorsProperlyInMultilevelParsing()
    {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Level0>(@"Field00,Field10,Field20,Field21,Field22,Field12,Field02
F00,11,Aaa,1978-03-19,Bbb,1978-11-11,19
G00,22,Ccc,2100-06-07,Ddd,2211-12-12,17
H00,22,Ccc,2100:06:07,Ddd,2211-12-12,17
I00,22,Ccc,2100-06-07,Ddd,2211-12-12,17
"));
        Assert.That(ex.LineNumber, Is.EqualTo(3));
        Assert.That(ex.LineText, Is.EqualTo("H00,22,Ccc,2100:06:07,Ddd,2211-12-12,17"));
        Assert.That(ex.TokenText, Is.EqualTo("2100:06:07"));
        Assert.That(ex.ParserTag, Is.EqualTo(nameof(DefaultDateOnlyParser)));
    }

    private Task<IEnumerable<TRow>> Read<TRow>(string input) where TRow : new()
    {
        return new CsvReadingEngineFactory().Create<TRow>().Read(input, CsvOptions.Builder().WithHeaders().Build());
    }
}