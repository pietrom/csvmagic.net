using CsvMagic;
using CsvMagic.Reading;

namespace Examples.Reading;

public static class WritableProperties {
    public static async Task Run() {
        Console.WriteLine("02-WritableProperties");
        var engine = new CsvReadingEngineFactory().Create<SampleRowContainingSetOnlyProperty>();
        var options = CsvOptions.Default();
        var data = await engine.ReadFromFile(options, "Reading/02-writable-properties.input.csv").ToListAsync();
        foreach (var item in data) {
            Console.WriteLine(item);
        }
        Console.WriteLine();
    }

    public record SampleRowContainingSetOnlyProperty {
        public int Id { get; set; }
        private string firstName = string.Empty;
        public string FirstName => firstName;
        private string lastName = string.Empty;
        public string LastName => lastName;

        public string FullName {
            set {
                var fields = value.Split(' ');
                firstName = fields[0];
                lastName = fields[1];
            }
        }
    }
}
