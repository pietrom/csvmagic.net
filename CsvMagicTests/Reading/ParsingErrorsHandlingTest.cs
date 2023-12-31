﻿using CsvMagic;
using CsvMagic.Reading;
using CsvMagic.Reading.Parsers;
using CsvMagicTests.DataTypes;

namespace CsvMagicTests.Reading;

public class ParsingErrorsHandlingTest {
    class Row {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    [Test]
    public Task ReadWithoutErrors() {
        return Read<Row>(@"Text,Value
A,1
B,2,
C,33333
");
    }

    [Test]
    public void ShouldTrackErrorLineNumber() {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.LineNumber, Is.EqualTo(3));
    }

    [Test]
    public void ShouldTrackErrorLineText() {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.LineText, Is.EqualTo("C,33xx333"));
    }

    [Test]
    public void ShouldTrackTokenText() {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.TokenText, Is.EqualTo("33xx333"));
    }

    [Test]
    public void ShouldTrackParserTag() {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.ParserTag, Is.EqualTo(nameof(DefaultIntParser)));
    }

    [Test]
    public void ShouldTrackMissingField() {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B
C,33
D,8765
"));
        Assert.That(ex.LineNumber, Is.EqualTo(2));
        Assert.That(ex.LineText, Is.EqualTo("B"));
        Assert.That(ex.ParserTag, Is.EqualTo(nameof(ComplexTypeParser<Row>)));
        Assert.That(ex.ErrorDetail, Is.EqualTo("Less Tokens Than Properties"));
    }

    [Test]
    public void ShouldTrackRestNotEmpty() {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read<Row>(@"Text,Value
A,1
B,11,AA,22
C,33
D,8765
"));
        Assert.That(ex.LineNumber, Is.EqualTo(2));
        Assert.That(ex.LineText, Is.EqualTo("B,11,AA,22"));
        Assert.That(ex.ParserTag, Is.EqualTo(nameof(CsvReadingEngine<Row>)));
        Assert.That(ex.ErrorDetail, Is.EqualTo("Rest Not Empty"));
    }

    [Test]
    public void ShouldTrackErrorsProperlyInMultilevelParsing() {
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

    private async Task<IEnumerable<TRow>> Read<TRow>(string input) where TRow : new() {
        return await new CsvReadingEngineFactory().Create<TRow>().ReadFromString(CsvOptions.Builder().WithHeaders().Build(), input).ToListAsync();
    }

    [Test]
    public void ShouldWrapExceptionIntoCsvReadingException() {
        var engine = new CsvReadingEngineFactory()
            .RegisterParser<int>(new FailingParser())
            .Create<Row>();

        var options = CsvOptions.Builder().WithoutHeaders().Build();
        Assert.ThrowsAsync<CsvReadingException>(() => engine.ReadFromString(options, "text,19").ToListAsync().AsTask());
    }
}

public class FailingParser : FieldParser {
    public (object?, string?) ParseNext(CsvReadingContext context, string? text) {
        throw new Exception("Fake exception");
    }
}
