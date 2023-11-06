using System.Text;
using CsvMagic;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

public class CsvWritingEngineTest
{
    private static readonly CsvOptions Options = CsvOptions.Builder().WithDelimiter(';').Build();

    [Test]
    public async Task SerializeCsv()
    {
        var engine = new CsvWritingEngineFactory()
                .AddSerializer<DateOnly>(new DateOnlyRenderer("yyyyMMdd"))
                .Create<CsvWriteData>()
            ;

        var stream = new MemoryStream();
        await engine.Write(new[]
        {
            new CsvWriteData { Counter = 1, LongValue = 19, StringValue = "pietrom", BirthDay = new DateOnly(1978, 3, 19), OtherDay = new DateOnly(2008, 5, 22), OtherString = "pietro;m"},
            new CsvWriteData { Counter = 2, LongValue = 11, StringValue = "cristinar", BirthDay = new DateOnly(1978, 11, 11), OtherDay = new DateOnly(2008, 5, 22), OtherString = "cristina;r"},
        }, new StreamWriter(stream), Options);
        stream.Seek(0, SeekOrigin.Begin);
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.That(result, Is.EqualTo(@"Counter;StringValue;LongValue;BirthDay;OtherDay;OtherString
1;pietrom;19;1978-03-19;20080522;""pietro;m""
2;cristinar;11;1978-11-11;20080522;""cristina;r""
"));
    }
}