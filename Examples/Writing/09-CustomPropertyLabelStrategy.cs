using System.Reflection;
using CsvMagic;
using CsvMagic.Writing;

namespace Examples.Writing;

public static class CustomPropertyLabelStrategy {
    public static Task Run() {
        var engine = new CsvWritingEngineFactory()
                .WithLabelStrategy(new UppercaseFieldLabelStrategy())
                .Create<RowData>()
                .Configure(x => x.When).UsingLabel("The date when the party starts")
            ;
        var options = CsvOptions.Default();
        var people = new[] {
            new RowData("pietrom", 19,new DateOnly(2023, 12, 3)),
            new RowData("martinellip", 11,new DateOnly(2023, 12, 3)),
            new RowData("pm", 22,new DateOnly(2023, 12, 3)),
        };
        return engine.WriteToFile(options, people, "09-custom-property-label-strategy.csv");
    }

    private record RowData(string Text, long Value, DateOnly When);

    private class UppercaseFieldLabelStrategy : FieldLabelWritingStrategy {
        public string GetLabel(PropertyInfo info) {
            return info.Name.ToUpperInvariant();
        }
    }
}
