using CsvMagic;
using CsvMagic.Writing.Renderers;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultDecimalRendererTest
{
    private readonly DefaultDecimalRenderer renderer = new DefaultDecimalRenderer();

    [TestCase(null, "")]
    [TestCase(1234, "1234")]
    [TestCase(1234.56, "1234.56")]
    public void RenderUsingDefaulta(decimal? input, string output)
    {
        var result = renderer.Render(new CsvRow().Options, input);
        Assert.That(result, Is.EqualTo(output));
    }

    [TestCase(null, "")]
    [TestCase(1234, "1234")]
    [TestCase(1234.56, "1234,56")]
    public void RenderCustom(decimal? input, string output)
    {
        var result = renderer.Render(new CsvOptions(';', '"', ',', false), input);
        Assert.That(result, Is.EqualTo(output));
    }


    [TestCase(1234.56, "\"1234,56\"")]
    public void RenderCustomWithQuotingNeeded(decimal? input, string output)
    {
        var result = renderer.Render(new CsvOptions(',', '"', ',', false), input);
        Assert.That(result, Is.EqualTo(output));
    }
}