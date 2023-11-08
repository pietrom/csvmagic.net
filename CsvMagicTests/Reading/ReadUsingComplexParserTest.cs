using System.Text;
using CsvMagic;
using CsvMagic.Reading;
using CsvMagic.Reading.Parsers;
using CsvMagicTests.DataTypes;

namespace CsvMagicTests.Reading;

[TestFixture]
public class ComplexTypeParserTest
{

    record Row
    {
        [CsvField(Parser = typeof(UsernameParser))]
        public Username Username { get; set; }
        public Address Address { get; set; }

        public int Value { get; set; }
    }

    [Test]
    public async Task Read()
    {
        var engine = new CsvReadingEngineFactory().Create<Row>();
        var result = (await engine.Read(@"pietrom,via delle Razziche,1,19", CsvOptions.Builder().WithoutHeaders().Build())).Single();
        Assert.That(result, Is.EqualTo(new Row { Username = new Username("pietrom"), Address = new Address("via delle Razziche", "1" ), Value = 19 }));
    }
}

class UsernameParser : QuotingParser<Username?>
{
    protected override Username? ParseValue(CsvReadingContext context, string? value)
    {
        return string.IsNullOrEmpty(value) ? null : new Username(value);
    }
}
