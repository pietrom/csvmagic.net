using System.Text;
using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

[TestFixture]
public class CsvReadingEngineTest {
    private CsvReadingEngine<CsvReadData> engine;

    [SetUp]
    public void InitEngine() {
        engine = new CsvReadingEngineFactory().Create<CsvReadData>();
        engine.Configure(x => x.CustomDateOnly).UsingParser(new CustomDateOnlyParser());
    }

    private async Task<List<CsvReadData>> ReadAsCsv(string input, CsvOptions? options = null) {
        return await engine.ReadFromStream(options ?? CsvOptions.Default(), new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(input))))
            .ToListAsync();
    }

    [Test]
    public async Task Read() {
        var input = @"Counter,StringValue,LongValue,BirthDay
1,pietrom,19,1978-03-19,19780319,
2,russocri,11,1978-11-11,19781111,1978-11-11
";
        var rows = await ReadAsCsv(input);
        Assert.That(rows, Is.EquivalentTo(new[]
        {
            new CsvReadData
            {
                Counter = 1, StringValue = "pietrom", LongValue = 19, DefaultDateOnly = new DateOnly(1978, 3, 19),
                CustomDateOnly = new DateOnly(1978, 3, 19), DefaultNullableDateOnly = null
            },
            new CsvReadData
            {
                Counter = 2, StringValue = "russocri", LongValue = 11, DefaultDateOnly = new DateOnly(1978, 11, 11),
                CustomDateOnly = new DateOnly(1978, 11, 11), DefaultNullableDateOnly = new DateOnly(1978, 11, 11)
            }
        }));
    }

    [Test]
    public async Task ReadWithoutHeaders() {
        var input = @"1,pietrom,19,1978-03-19,19780319,
2,russocri,11,1978-11-11,19781111,1978-11-11
";
        var rows = await ReadAsCsv(input, CsvOptions.Builder().WithoutHeaders().Build());
        Assert.That(rows, Is.EquivalentTo(new[]
        {
            new CsvReadData
            {
                Counter = 1, StringValue = "pietrom", LongValue = 19, DefaultDateOnly = new DateOnly(1978, 3, 19),
                CustomDateOnly = new DateOnly(1978, 3, 19), DefaultNullableDateOnly = null
            },
            new CsvReadData
            {
                Counter = 2, StringValue = "russocri", LongValue = 11, DefaultDateOnly = new DateOnly(1978, 11, 11),
                CustomDateOnly = new DateOnly(1978, 11, 11), DefaultNullableDateOnly = new DateOnly(1978, 11, 11)
            }
        }));
    }

    [Test]
    public async Task ReadUsingCustomDelimiter() {
        var input = @"Counter;StringValue;LongValue;BirthDay
1;pietrom;19;1978-03-19;19780319;
2;russocri;11;1978-11-11;19781111;1978-11-11
";
        var options = CsvOptions.Builder()
            .WithHeaders()
            .WithDelimiter(';')
            .Build();
        var rows = await ReadAsCsv(input, options);
        Assert.That(rows, Is.EquivalentTo(new[]
        {
            new CsvReadData
            {
                Counter = 1, StringValue = "pietrom", LongValue = 19, DefaultDateOnly = new DateOnly(1978, 3, 19),
                CustomDateOnly = new DateOnly(1978, 3, 19), DefaultNullableDateOnly = null
            },
            new CsvReadData
            {
                Counter = 2, StringValue = "russocri", LongValue = 11, DefaultDateOnly = new DateOnly(1978, 11, 11),
                CustomDateOnly = new DateOnly(1978, 11, 11), DefaultNullableDateOnly = new DateOnly(1978, 11, 11)
            }
        }));
    }
}
