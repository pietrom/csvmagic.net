using CsvMagic;
using CsvMagic.Reading;
using CsvMagicTests.DataTypes;

namespace CsvMagicTests.Reading;

public class ReadAllTest {
    [Test]
    public async Task Read() {
        var input = @"test,-19,11,22.17,17.22,-3,100,-4046.4949,1,2006-05-22,2008-05-17T16:00:00.0000000+02:00
";
        var engine = new CsvReadingEngineFactory().Create<AllData>();
        var rows = await engine.ReadFromString(CsvOptions.Builder().WithoutHeaders().Build(), input).ToListAsync();
        Assert.That(rows, Is.EquivalentTo(new[]
        {
            new AllData
            {
                Text = "test",
                IntValue = -19,
                LongValue = 11,
                DecimalValue = 22.17m,
                DoubleValue = 17.22,
                ShortValue = -3,
                UintValue = 100,
                FloatValue = -4046.4949f,
                BoolValue = true,
                DateOnlyValue = new DateOnly(2006, 5, 22),
                DateTimeOffsetValue = new DateTimeOffset(2008, 5, 17, 16, 0, 0, TimeSpan.FromHours(2))
            }
        }));
    }
}
