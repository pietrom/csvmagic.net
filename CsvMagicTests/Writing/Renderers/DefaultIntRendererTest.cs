using CsvMagic;
using CsvMagic.Writing.Renderers;
using static CsvMagicTests.Writing.CsvWritingContextHelper;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultIntRendererTest {
    private readonly DefaultIntRenderer renderer = new DefaultIntRenderer();

    [TestCase(null, "")]
    [TestCase(1234, "1234")]
    [TestCase(-1234, "-1234")]
    public void RenderUsingDefaulta(int? input, string output) {
        var result = renderer.RenderObject(ContextFrom(CsvOptions.Default()), input);
        Assert.That(result, Is.EqualTo(output));
    }

    [TestCase(-1234, "\"-1234\"")]
    public void RenderCustomWithQuotingNeeded(int? input, string output) {
        var options = CsvOptions.Builder()
            .WithDelimiter('-')
            .WithDecimalSeparator(',')
            .WithoutHeaders()
            .Build();
        var result = renderer.RenderObject(ContextFrom(options), input);
        Assert.That(result, Is.EqualTo(output));
    }
}
