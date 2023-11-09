using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

public class ParsingErrorsHandlingTest
{
    private readonly CsvReadingEngine<Row> engine = new CsvReadingEngineFactory().Create<Row>();

    class Row
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    [Test]
    public Task ReadWithoutErrors()
    {
        return Read(@"Text,Value
A,1
B,2,C,33333
");
    }

    [Test]
    public void ShouldTrackErrorLineNumber()
    {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.ErrorLineNumber, Is.EqualTo(3));
    }

    [Test]
    public void ShouldTrackErrorLineText()
    {
        var ex = Assert.ThrowsAsync<CsvReadingException>(() => Read(@"Text,Value
A,1
B,2,
C,33xx333
D,8765
"));
        Assert.That(ex.ErrorLineText, Is.EqualTo("C,33xx333"));
    }

    private Task<IEnumerable<Row>> Read(string input)
    {
        return engine.Read(input, CsvOptions.Builder().WithHeaders().Build());
    }


    
}