using CsvMagic;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

[TestFixture]
public class WriteOnlyGettablePropertiesTest
{
    [CsvRow]
    class Row
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName {
            set
            {
                var fields = value.Split(' ');
                FirstName = fields[0];
                LastName = fields[1];
            }
        }
    }

    [Test]
    public async Task ShouldNotWriteSetterOnlyProperty()
    {
        var engine = new CsvWritingEngineFactory().Create<Row>();
        var result = await engine.Write(new[] { new Row { FullName = "Pietro Martinelli" }});
        Assert.That(result, Is.EqualTo(@"FirstName,LastName
Pietro,Martinelli
"));
    }
}