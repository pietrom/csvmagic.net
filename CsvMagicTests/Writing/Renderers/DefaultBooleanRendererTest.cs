using CsvMagic;
using CsvMagic.Writing.Renderers;
using static CsvMagicTests.Writing.CsvWritingContextHelper;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultBooleanRendererTest
{
    [TestCase(null, "")]
    [TestCase(true, "1")]
    [TestCase(false, "0")]
    public void RenderUsingDefaults(bool? input, string output)
    {
        var result = new DefaultBooleanRenderer().RenderObject(ContextFrom(CsvOptions.Default()), input);
        Assert.That(result, Is.EqualTo(output));
    }

    [TestCase(null, "")]
    [TestCase(true, "+")]
    [TestCase(false, "-")]
    public void RenderUsingCustomText(bool? input, string output)
    {
        var result = new DefaultBooleanRenderer("+", "-").RenderObject(ContextFrom(CsvOptions.Default()), input);
        Assert.That(result, Is.EqualTo(output));
    }
}