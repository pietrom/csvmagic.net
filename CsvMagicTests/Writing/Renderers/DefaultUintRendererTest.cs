using CsvMagic;
using CsvMagic.Writing.Renderers;
using static CsvMagicTests.Writing.CsvWritingContextHelper;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultUintRendererTest {
    private readonly DefaultUintRenderer renderer = new ();

    [TestCase(null, "")]
    [TestCase(1234u, "1234")]
    public void RenderUsingDefaults(uint? input, string output) {
        var result = renderer.RenderObject(ContextFrom(CsvOptions.Default()), input);
        Assert.That(result, Is.EqualTo(output));
    }
}
