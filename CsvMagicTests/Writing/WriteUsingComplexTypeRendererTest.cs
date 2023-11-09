using CsvMagic;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

[TestFixture]
public class WriteUsingComplexTypeRendererTest
{
    class Level0
    {
        public string Field00 { get; set; }
        public Level1 Field01 { get; set; }
        public int Field02 { get; set; }
    }

    class Level1
    {
        public int Field10 { get; set; }
        public Level2 Field11 { get; set; }
        public DateOnly Field12 { get; set; }
    }

    class Level2
    {
        public string Field20 { get; set; }
        public DateOnly Field21 { get; set; }
        public string Field22 { get; set; }
    }

    [Test]
    public async Task ShouldNotWriteSetterOnlyProperty()
    {
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