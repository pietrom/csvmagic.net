using CsvMagic;
using CsvMagic.Writing.Renderers;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultFloatRendererTest {
    private readonly DefaultFloatRenderer renderer = new ();

    [TestCase(null, "")]
    [TestCase(1234f, "1234")]
    [TestCase(1234.56f, "1234.56")]
    [TestCase(-1234.56f, "-1234.56")]
    public void RenderUsingDefaulta(float? input, string output) {
        var result = renderer.RenderObject(CsvWritingContextHelper.ContextFrom(CsvOptions.Default()), input);
        Assert.That(result, Is.EqualTo(output));
    }

    [TestCase(null, "")]
    [TestCase(1234f, "1234")]
    [TestCase(1234.56f, "1234,56")]
    public void RenderCustom(float? input, string output) {
        var result = renderer.RenderObject(CsvWritingContextHelper.ContextFrom(new CsvOptions(';', '"', ',', false, false)), input);
        Assert.That(result, Is.EqualTo(output));
    }


    [TestCase(-1234.56f, "\"-1234,56\"")]
    public void RenderCustomWithQuotingNeeded(float? input, string output) {
        var result = renderer.RenderObject(CsvWritingContextHelper.ContextFrom(new CsvOptions('-', '"', ',', false, false)), input);
        Assert.That(result, Is.EqualTo(output));
    }
}
