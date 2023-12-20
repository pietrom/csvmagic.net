using CsvMagic;
using CsvMagic.Writing.Renderers;
using static CsvMagicTests.Writing.CsvWritingContextHelper;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultStringRendererTest {
    private readonly DefaultStringRenderer renderer = new ();

    [TestCase(null, "")]
    [TestCase("", "")]
    [TestCase("abcd", "abcd")]
    [TestCase("ab,cd", "\"ab,cd\"")]
    public void RenderUsingDefaults(string? input, string output) {
        var result = renderer.RenderObject(ContextFrom(CsvOptions.Default()), input);
        Assert.That(result, Is.EqualTo(output));
    }
}
