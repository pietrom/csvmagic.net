using CsvMagic.Helpers;

namespace CsvMagicTests.Helpers;

[TestFixture]
public class DictionaryExtensionTest {
    private static readonly IDictionary<string, string> Dictionary = new Dictionary<string, string> {
        { "foo", "bar" }
    };

    [Test]
    public void GetExistingKeyProvidingDefaultValue() {
        Assert.That(Dictionary.GetOrDefault("foo", "default-foo"), Is.EqualTo("bar"));
    }

    [Test]
    public void GetNotExistingKeyProvidingDefaultValue() {
        Assert.That(Dictionary.GetOrDefault("other-key", "default-foo"), Is.EqualTo("default-foo"));
    }

    [Test]
    public void GetExistingKeyProvidingDefaultValueGenerator() {
        Assert.That(Dictionary.GetOrDefault("foo", () => "default-foo"), Is.EqualTo("bar"));
    }

    [Test]
    public void GetNotExistingKeyProvidingDefaultValueGenerator() {
        Assert.That(Dictionary.GetOrDefault("other-key", () => "default-foo"), Is.EqualTo("default-foo"));
    }
}
