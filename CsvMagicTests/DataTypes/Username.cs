namespace CsvMagicTests.DataTypes;

public record Username(string Value);

public record Address
{
    public Address(string street, string number)
    {
        Street = street;
        Number = number;
    }

    public Address()
    {
    }

    public string Street { get; set; }
    public string Number { get; set; }
}