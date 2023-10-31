using CsvMagic.Reading.Parsers;

namespace CsvMagicTests.Reading;

[TestFixture]
public class DefaultStringParserTest
{
    private readonly DefaultStringParser parser = new();

    [TestCase(null, "")]
    [TestCase("", "")]
    public void Run(string input, string output)
    {
        var result = parser.Parse(input);
        Assert.That(result, Is.EqualTo(output));
    }
}