using CsvMagic;
using CsvMagic.Writing;
using Samples.WriteCsv;

public static class RecursiveRenderingProgram {
    public static async Task Execute() {
        var engine = new CsvWritingEngineFactory().Create<Pet>();

        var rows = new[] {
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
            }
        };

        await engine.Write(rows, new StreamWriter(File.OpenWrite("pets.csv")), CsvOptions.Default());
    }
}
