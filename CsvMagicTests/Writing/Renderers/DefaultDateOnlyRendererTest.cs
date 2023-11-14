using CsvMagic;
using CsvMagic.Writing.Renderers;
using static CsvMagicTests.Writing.CsvWritingContextHelper;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultDateOnlyRendererTest {
    private readonly DefaultDateOnlyRenderer renderer = new DefaultDateOnlyRenderer();

    [Test]
    public void RenderNullValueToEmptyString() {
        Assert.That(renderer.RenderObject(ContextFrom(CsvOptions.Default()), null), Is.EqualTo(string.Empty));
    }

    [Test]
    public void RenderValue() {
        Assert.That(renderer.RenderObject(ContextFrom(CsvOptions.Default()), new DateOnly(2023, 11, 5)), Is.EqualTo("2023-11-05"));
    }


    [Test]
    public void RenderValueWithQuotingNeeded() {
        Assert.That(renderer.RenderObject(ContextFrom(new CsvOptions('-', '"', '.', false)), new DateOnly(2023, 11, 5)), Is.EqualTo("\"2023-11-05\""));
    }
}
