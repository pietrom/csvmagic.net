using System.Reflection;
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
        await engine.WriteToStream(Options, new[]
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
    public async Task SerializeCsvWithCustomLabelsThroughAttribute() {
        var engine = new CsvWritingEngineFactory()
                .RegisterRenderer<DateOnly>(new DateOnlyRenderer("yyyyMMdd"))
                .Create<CsvWriteDataWithCustomLabels>()
            ;

        var stream = new MemoryStream();
        await engine.WriteToStream(Options, new[]
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
    public async Task SerializeCsvWithCustomLabelsThroughExplicitConfiguration() {
        var engine = new CsvWritingEngineFactory()
                .Create<CsvWriteData>()
            ;

        engine.Configure(x => x.BirthDay).UsingLabel("The day of your birth");
        engine.Configure(x => x.StringValue).UsingLabel("TextValue");
        engine.Configure(x => x.OtherDay).UsingLabel("Another Day");
        engine.Configure(x => x.OtherString).UsingLabel("Another String");

        var stream = new MemoryStream();
        await engine.WriteToStream(Options, new[]
        {
            new CsvWriteData { Counter = 1, LongValue = 19, StringValue = "pietrom", BirthDay = new DateOnly(1978, 3, 19), OtherDay = new DateOnly(2008, 5, 22), OtherString = "pietro;m"},
            new CsvWriteData { Counter = 2, LongValue = 11, StringValue = "cristinar", BirthDay = new DateOnly(1978, 11, 11), OtherDay = new DateOnly(2008, 5, 22), OtherString = "cristina;r"},
        }, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.That(result, Is.EqualTo(@"Counter;TextValue;LongValue;The day of your birth;Another Day;Another String
1;pietrom;19;1978-03-19;2008-05-22;""pietro;m""
2;cristinar;11;1978-11-11;2008-05-22;""cristina;r""
"));
    }

    [Test]
    public async Task SerializeCsvWithCustomLabelsAndGenericLabelStrategy() {
        var engine = new CsvWritingEngineFactory()
                .WithLabelStrategy(new UppercaseLabelFactory())
                .Create<CsvWriteData>()
            ;

        engine.Configure(x => x.BirthDay).UsingLabel("The day of your birth");

        var stream = new MemoryStream();
        await engine.WriteToStream(Options, new[]
        {
            new CsvWriteData { Counter = 1, LongValue = 19, StringValue = "pietrom", BirthDay = new DateOnly(1978, 3, 19), OtherDay = new DateOnly(2008, 5, 22), OtherString = "pietro;m"},
            new CsvWriteData { Counter = 2, LongValue = 11, StringValue = "cristinar", BirthDay = new DateOnly(1978, 11, 11), OtherDay = new DateOnly(2008, 5, 22), OtherString = "cristina;r"},
        }, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.That(result, Is.EqualTo(@"COUNTER;STRINGVALUE;LONGVALUE;The day of your birth;OTHERDAY;OTHERSTRING
1;pietrom;19;1978-03-19;2008-05-22;""pietro;m""
2;cristinar;11;1978-11-11;2008-05-22;""cristina;r""
"));
    }

    [Test]
    public async Task SerializeCsvWithCustomLabelsAndRendererWithoutAttributes() {
        var engine = new CsvWritingEngineFactory()
            .RegisterRenderer<DateOnly>(new DateOnlyRenderer("yyyyMMdd"))
            .Create<CsvWriteDataPoco>();
            engine.Configure(x => x.StringValue).UsingLabel("TextValue")
                .Configure(x => x.BirthDay).UsingRenderer(new DateOnlyRenderer())
                .Configure(x => x.OtherDay).UsingLabel("Another Day")
                .Configure(x => x.OtherString).UsingLabel("Another String");

        var stream = new MemoryStream();
        await engine.WriteToStream(Options, new[]
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

    class UppercaseLabelFactory : FieldLabelWritingStrategy {
        public string GetLabel(PropertyInfo info) {
            return info.Name.ToUpperInvariant();
        }
    }
}
