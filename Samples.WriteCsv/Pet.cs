namespace Samples.WriteCsv;

public class Pet {
    public string Name { get; set; }
    public Person Owner { get; set; }
    public string Color { get; set; }
}

public class Person {
    public string FullName { get; set; }
    public Address Address { get; set; }
    public int Age { get; set; }
}

public class Address {
    public string Street { get; set; }
    public string Number { get; set; }
}
