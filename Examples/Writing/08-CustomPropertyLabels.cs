using CsvMagic;
using CsvMagic.Writing;
using CsvMagic.Writing.Renderers;

namespace Examples.Writing;

public static class CustomPropertyLabels {
    public static Task Run() {
        var engine = new CsvWritingEngineFactory()
            .Create<RowData>()
            .Configure(x => x.Value).UsingLabel("The long value")
            .Configure(x => x.When).UsingLabel("The date when the party starts")
            ;
        var options = CsvOptions.Default();
        var people = new[] {
            new RowData("pietrom", 19,new DateOnly(2023, 12, 3)),
            new RowData("martinellip", 11,new DateOnly(2023, 12, 3)),
            new RowData("pm", 22,new DateOnly(2023, 12, 3)),
        };
        return engine.WriteToFile(options, people, "08-custom-property-labels.csv");
    }

    private record RowData(string Text, long Value, DateOnly When);
}
