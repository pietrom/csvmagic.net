using CsvMagic;
using CsvMagic.Writing;
using CsvMagicTests.DataTypes;

namespace CsvMagicTests.Writing;

[TestFixture]
public class WriteUsingComplexTypeRendererTest {
    [Test]
    public async Task ShouldNotWriteSetterOnlyProperty() {
        var engine = new CsvWritingEngineFactory().Create<Level0>();
        var result = await engine.Write(new[] { new Level0 { Field00 = "F00", Field02 = 19, Field01 = new Level1
        {
            Field10 = 11,
            Field12 = new DateOnly(1978, 11, 11),
            Field11 = new Level2
            {
                Field20 = "Aaa",
                Field21 = new DateOnly(1978,3,19),
                Field22 = "Bbb"
            }
        }}});
        Assert.That(result, Is.EqualTo(@"Field00,Field10,Field20,Field21,Field22,Field12,Field02
F00,11,Aaa,1978-03-19,Bbb,1978-11-11,19
"));
    }
}
