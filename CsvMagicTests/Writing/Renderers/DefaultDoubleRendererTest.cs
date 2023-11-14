using CsvMagic;
using CsvMagic.Writing.Renderers;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultDoubleRendererTest {
    private readonly DefaultDoubleRenderer renderer = new DefaultDoubleRenderer();

    [TestCase(null, "")]
    [TestCase(1234, "1234")]
    [TestCase(1234.56, "1234.56")]
    public void RenderUsingDefaulta(double? input, string output) {
        var result = renderer.RenderObject(CsvWritingContextHelper.ContextFrom(CsvOptions.Default()), input);
        Assert.That(result, Is.EqualTo(output));
    }

    [TestCase(null, "")]
    [TestCase(1234, "1234")]
    [TestCase(1234.56, "1234,56")]
    public void RenderCustom(double? input, string output) {
        var result = renderer.RenderObject(CsvWritingContextHelper.ContextFrom(new CsvOptions(';', '"', ',', false)), input);
        Assert.That(result, Is.EqualTo(output));
    }


    [TestCase(1234.56, "\"1234,56\"")]
    public void RenderCustomWithQuotingNeeded(double? input, string output) {
        var result = renderer.RenderObject(CsvWritingContextHelper.ContextFrom(new CsvOptions(',', '"', ',', false)), input);
        Assert.That(result, Is.EqualTo(output));
    }
}
