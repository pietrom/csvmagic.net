using CsvMagic;
using CsvMagic.Writing.Renderers;

namespace CsvMagicTests.Writing.Renderers;

[TestFixture]
public class DefaultDateTimeOffsetRendererTest
{
    private readonly DefaultDateTimeOffsetRenderer renderer = new DefaultDateTimeOffsetRenderer();

    [Test]
    public void RenderNullValueToEmptyString()
    {
        Assert.That(renderer.Render(new CsvRow().Options, null), Is.EqualTo(string.Empty));
    }

    [Test]
    public void RenderValueToIso8601String()
    {
        Assert.That(renderer.Render(new CsvRow().Options, new DateTimeOffset(2023, 11, 5, 13, 24, 43, 123, TimeSpan.FromHours(-3))), Is.EqualTo("2023-11-05T13:24:43.1230000-03:00"));
    }
}