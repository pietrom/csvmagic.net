using CsvMagic;
using CsvMagic.Reading;

namespace Examples.Reading;

public static class RecursiveParsing {
    public static async Task Run() {
        Console.WriteLine("03-RecursiveParsing");
        var engine = new CsvReadingEngineFactory().Create<Pet>();
        var options = CsvOptions.Default();
        var data = await engine.ReadFromFile(options, "Reading/03-recursive-parsing.input.csv").ToListAsync();
        foreach (var item in data) {
            Console.WriteLine(item);
        }
        Console.WriteLine();
    }

    public record Pet {
        public string Name { get; set; }
        public Person Owner { get; set; }
        public string Color { get; set; }
    }

    public record Person {
        public string FullName { get; set; }
        public Address Address { get; set; }
        public int Age { get; set; }
    }

    public record Address {
        public string Street { get; set; }
        public string Number { get; set; }
    }
}
