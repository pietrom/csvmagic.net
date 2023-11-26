using CsvMagic;
using CsvMagic.Writing.Renderers;
using static CsvMagicTests.Writing.CsvWritingContextHelper;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultDateTimeOffsetRendererTest {
    private readonly DefaultDateTimeOffsetRenderer renderer = new DefaultDateTimeOffsetRenderer();

    [Test]
    public void RenderNullValueToEmptyString() {
        Assert.That(renderer.RenderObject(ContextFrom(CsvOptions.Default()), null), Is.EqualTo(string.Empty));
    }

    [Test]
    public void RenderValueToIso8601String() {
        Assert.That(renderer.RenderObject(ContextFrom(CsvOptions.Default()), new DateTimeOffset(2023, 11, 5, 13, 24, 43, 123, TimeSpan.FromHours(-3))), Is.EqualTo("2023-11-05T13:24:43.1230000-03:00"));
    }


    [Test]
    public void RenderValueToIso8601StringWithQuotingNeeded() {
        Assert.That(renderer.RenderObject(ContextFrom(new CsvOptions('-', '"', '.', false, false)), new DateTimeOffset(2023, 11, 5, 13, 24, 43, 123, TimeSpan.FromHours(-3))), Is.EqualTo("\"2023-11-05T13:24:43.1230000-03:00\""));
    }
}
