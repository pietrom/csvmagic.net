using CsvMagic;
using CsvMagic.Writing;

namespace Examples.Writing;

public static class ReadableProperties {
    public static Task Run() {
        var engine = new CsvWritingEngineFactory().Create<PersonData>();
        var options = CsvOptions.Default();
        var people = new[] {
            new PersonData("Pietro", "Martinelli", new DateOnly(1978, 3, 19)),
            new PersonData("Charles", "Darwin", new DateOnly(1809, 2, 12)),
        };
        return engine.WriteToFile(options, people, "02-readable-properties.csv");
    }

    private record PersonData(string FirstName, string LastName, DateOnly Birthday) {
        public string Initials => FirstName.Substring(0, 1) + LastName.Substring(0, 1);

        public int Age => DateOnly.FromDateTime(DateTime.Now).Year - Birthday.Year;
    }
}
