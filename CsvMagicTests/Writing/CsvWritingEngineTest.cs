using System.Text;
using CsvMagic;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

public class CsvWritingEngineTest {
    private static readonly CsvOptions Options = CsvOptions.Builder().WithDelimiter(';').Build();

    [Test]
    public async Task SerializeCsvUsingCustomRendererToo() {
        var engine = new CsvWritingEngineFactory()
                .RegisterRenderer<DateOnly>(new DateOnlyRenderer("yyyyMMdd"))
                .Create<CsvWriteData>()
            ;

        var stream = new MemoryStream();
        await engine.Write(Options, new[]
        {
            new CsvWriteData { Counter = 1, LongValue = 19, StringValue = "pietrom", BirthDay = new DateOnly(1978, 3, 19), OtherDay = new DateOnly(2008, 5, 22), OtherString = "pietro;m"},
            new CsvWriteData { Counter = 2, LongValue = 11, StringValue = "cristinar", BirthDay = new DateOnly(1978, 11, 11), OtherDay = new DateOnly(2008, 5, 22), OtherString = "cristina;r"},
        }, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.That(result, Is.EqualTo(@"Counter;StringValue;LongValue;BirthDay;OtherDay;OtherString
1;pietrom;19;1978-03-19;20080522;""pietro;m""
2;cristinar;11;1978-11-11;20080522;""cristina;r""
"));
    }

    [Test]
    public async Task SerializeCsvWithCustomLabels() {
        var engine = new CsvWritingEngineFactory()
                .RegisterRenderer<DateOnly>(new DateOnlyRenderer("yyyyMMdd"))
                .Create<CsvWriteDataWithCustomLabels>()
            ;

        var stream = new MemoryStream();
        await engine.Write(Options, new[]
        {
            new CsvWriteDataWithCustomLabels { Counter = 1, LongValue = 19, StringValue = "pietrom", BirthDay = new DateOnly(1978, 3, 19), OtherDay = new DateOnly(2008, 5, 22), OtherString = "pietro;m"},
            new CsvWriteDataWithCustomLabels { Counter = 2, LongValue = 11, StringValue = "cristinar", BirthDay = new DateOnly(1978, 11, 11), OtherDay = new DateOnly(2008, 5, 22), OtherString = "cristina;r"},
        }, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.That(result, Is.EqualTo(@"Counter;TextValue;LongValue;BirthDay;Another Day;Another String
1;pietrom;19;1978-03-19;20080522;""pietro;m""
2;cristinar;11;1978-11-11;20080522;""cristina;r""
"));
    }

    [Test]
    public async Task SerializeCsvWithCustomLabelsAndRendererWithoutAttributes() {
        var engine = new CsvWritingEngineFactory()
            .RegisterRenderer<DateOnly>(new DateOnlyRenderer("yyyyMMdd"))
            .Create<CsvWriteDataPoco>();
            engine.Configure(x => x.StringValue).UsingLabel("TextValue");
            engine.Configure(x => x.BirthDay).UsingRenderer(new DateOnlyRenderer());
            engine.Configure(x => x.OtherDay).UsingLabel("Another Day");
            engine.Configure(x => x.OtherString).UsingLabel("Another String");
        /*
            [CsvField(Label = "TextValue")]
            public string? StringValue { get; set; }
            public long LongValue { get; set; }
            [CsvField(Renderer = typeof(DateOnlyRenderer))]
            public DateOnly BirthDay { get; set; }
            [CsvField(Label = "Another Day")]
            public DateOnly OtherDay { get; set; }
            [CsvField(Label = "Another String")]
            public string? OtherString { get; set; }
         */
        var stream = new MemoryStream();
        await engine.Write(Options, new[]
        {
            new CsvWriteDataPoco { Counter = 1, LongValue = 19, StringValue = "pietrom", BirthDay = new DateOnly(1978, 3, 19), OtherDay = new DateOnly(2008, 5, 22), OtherString = "pietro;m"},
            new CsvWriteDataPoco { Counter = 2, LongValue = 11, StringValue = "cristinar", BirthDay = new DateOnly(1978, 11, 11), OtherDay = new DateOnly(2008, 5, 22), OtherString = "cristina;r"},
        }, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.That(result, Is.EqualTo(@"Counter;TextValue;LongValue;BirthDay;Another Day;Another String
1;pietrom;19;1978-03-19;20080522;""pietro;m""
2;cristinar;11;1978-11-11;20080522;""cristina;r""
"));
    }
}
