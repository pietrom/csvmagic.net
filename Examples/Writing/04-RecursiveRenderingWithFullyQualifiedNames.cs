using CsvMagic;
using CsvMagic.Writing;

namespace Examples.Writing;

public static class RecursiveRenderingWIthFullyQualifiedNames {
    public static Task Run() {
        var engine = new CsvWritingEngineFactory().Create<Pet>();
        var options = CsvOptions.Builder().FullyQualifyNestedProperties().Build();
        var data = new[] {
            new Pet {
                Name = "Snow",
                Owner = new Person {
                    FullName = "Pietro Martinelli", Address = new Address { Street = "Main Street", Number = "19/C" },
                    Age = 45
                },
                Color = "Black"
            },
            new Pet {
                Name = "Birba",
                Owner = new Person {
                    FullName = "Gargamella", Address = new Address { Street = "Wood Street", Number = "11" }, Age = 115
                },
                Color = "Brown"
            },
        };;
        return engine.WriteToFile(options, data, "04-recursive-rendering-with-fully-qualified-names.csv");
    }

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
}
