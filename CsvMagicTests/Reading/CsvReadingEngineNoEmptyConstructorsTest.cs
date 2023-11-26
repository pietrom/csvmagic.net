using System.Text;
using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

[TestFixture]
public class CsvReadingEngineNoEmptyConstructorsTest {
    record Person(string FullName, Address Address, long Age);

    record Address(Street Street, string City);

    record Street(string Name, string Number);

    private CsvReadingEngine<Person> engine;

    [SetUp]
    public void InitEngine() {
        engine = new CsvReadingEngineFactory().Create<Person>(fields => {
            var fullName = (string)fields[nameof(Person.FullName)];
            var address = (Address)fields[nameof(Person.Address)];
            var age = (long)fields[nameof(Person.Age)];
            return new Person(fullName, address, age);
        });
        engine.Configure(x => x.Address).UsingFactory(fields => new Address((Street)fields["Street"], (string)fields["City"]));
        engine.Configure(x => x.Address.Street).UsingFactory(fields => new Street((string)fields["Name"], (string)fields["Number"]));
    }

    [Test]
    public async Task Read() {
        var input = @"Pietro Martinelli,via Martinelli,Brescia,19,45
";
        var person = await engine.ReadFromString(CsvOptions.Builder().WithoutHeaders().Build(), input).SingleAsync();
        Assert.That(person, Is.EqualTo(new Person("Pietro Martinelli", new Address(new Street("via Martinelli", "Brescia"), "19"), 45)));
    }
}
