using CsvMagic;
using CsvMagic.Writing.Renderers;
using static CsvMagicTests.Writing.CsvWritingContextHelper;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultShortRendererTest {
    private readonly DefaultShortRenderer renderer = new ();

    [TestCase(null, "")]
    [TestCase(1234, "1234")]
    [TestCase(-1234, "-1234")]
    public void RenderUsingDefaulta(short? input, string output) {
        var result = renderer.RenderObject(ContextFrom(CsvOptions.Default()), input);
        Assert.That(result, Is.EqualTo(output));
    }

    [TestCase(-1234, "\"-1234\"")]
    public void RenderCustomWithQuotingNeeded(short? input, string output) {
        var result = renderer.RenderObject(ContextFrom(new CsvOptions('-', '"', ',', false, false)), input);
        Assert.That(result, Is.EqualTo(output));
    }
}
