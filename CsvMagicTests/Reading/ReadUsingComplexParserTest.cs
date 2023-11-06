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
        [CsvField(Parser = typeof(AddressParser))]
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
    protected override Username? ParseValue(CsvOptions options, string? value)
    {
        return string.IsNullOrEmpty(value) ? null : new Username(value);
    }
}

class AddressParser : ComplexTypeParser<Address?>
{
    protected override IList<FieldParser> SubParsers => new[] { new DefaultStringParser(), new DefaultStringParser() };

    protected override Address? Build(object?[] parameters)
    {
        return new Address(parameters[0] as string, parameters[1] as string);
    }
}