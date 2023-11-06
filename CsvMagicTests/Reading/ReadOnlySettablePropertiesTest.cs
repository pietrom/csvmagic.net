using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

[TestFixture]
public class ReadOnlySettablePropertiesTest
{
    record Row
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Sum => X + Y;
    }

    [Test]
    public async Task ShouldNotReadGetterOnlyProperty()
    {
        var engine = new CsvReadingEngineFactory().Create<Row>();
        var result = (await engine.Read(@"11,19", CsvOptions.Builder().WithoutHeaders().Build())).Single();
        Assert.That(result, Is.EqualTo(new Row { X = 11, Y = 19 }));
    }
}