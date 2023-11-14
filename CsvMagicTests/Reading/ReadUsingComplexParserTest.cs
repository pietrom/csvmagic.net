using CsvMagic;
using CsvMagic.Reading;
using CsvMagic.Reading.Parsers;
using CsvMagicTests.DataTypes;

namespace CsvMagicTests.Reading;

[TestFixture]
public class ComplexTypeParserTest {
    record Row {
        [CsvField(Parser = typeof(UsernameParser))]
        public Username Username { get; set; }

        public Address Address { get; set; }

        public int Value { get; set; }
    }

    [Test]
    public async Task Read() {
        var engine = new CsvReadingEngineFactory().Create<Row>();
        var result =
            (await engine.Read(@"pietrom,via delle Razziche,1,19", CsvOptions.Builder().WithoutHeaders().Build()))
            .Single();
        Assert.That(result,
            Is.EqualTo(new Row { Username = new Username("pietrom"), Address = new Address("via delle Razziche", "1"), Value = 19 }));
    }

    [Test]
    public async Task MultiLevelRead() {
        var engine = new CsvReadingEngineFactory().Create<Level0>();
        var result = (await engine.Read(@"Field00,Field10,Field20,Field21,Field22,Field12,Field02
F00,11,Aaa,1978-03-19,Bbb,1978-11-11,19
G00,22,Ccc,2100-06-07,Ddd,2211-12-12,17
", CsvOptions.Builder().WithHeaders().Build()));
        var first = result.First();
        var second = result.Last();
        Assert.That(first, Is.EqualTo(new Level0 {
            Field00 = "F00",
            Field01 = new Level1 {
                Field10 = 11,
                Field11 = new Level2 {
                    Field20 = "Aaa",
                    Field21 = new DateOnly(1978, 3, 19),
                    Field22 = "Bbb"
                },
                Field12 = new DateOnly(1978, 11, 11)
            },
            Field02 = 19
        }));
        Assert.That(second, Is.EqualTo(new Level0 {
            Field00 = "G00",
            Field01 = new Level1 {
                Field10 = 22,
                Field11 = new Level2 {
                    Field20 = "Ccc",
                    Field21 = new DateOnly(2100, 6, 7),
                    Field22 = "Ddd"
                },
                Field12 = new DateOnly(2211, 12, 12)
            },
            Field02 = 17
        }));
    }
}

class UsernameParser : QuotingParser<Username?> {
    protected override Username? ParseValue(CsvReadingContext context, string? value) {
        return string.IsNullOrEmpty(value) ? null : new Username(value);
    }
}
