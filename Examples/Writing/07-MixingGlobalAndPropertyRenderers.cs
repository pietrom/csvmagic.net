using CsvMagic;
using CsvMagic.Writing;
using CsvMagic.Writing.Renderers;

namespace Examples.Writing;

public static class MixingGlobalAndPropertyRenderers {
    public static Task Run() {
        var engine = new CsvWritingEngineFactory()
                .RegisterRenderer<DateOnly>(new DateOnlyRenderer("dd-MM-yyyy"))
            .Create<RowData>()
            .Configure(x => x.DateWithPropertyCustomFormat).UsingRenderer(new DateOnlyRenderer("yyyyMMdd"))
            .Configure(x => x.DateWithAnotherPropertyCustomFormat).UsingRenderer(new DateOnlyRenderer("dd/MM/yyyy"))
            ;
        var options = CsvOptions.Default();
        var people = new[] {
            new RowData(new DateOnly(2023, 12, 3), new DateOnly(2023, 12, 3), new DateOnly(2023, 12, 3)),
        };
        return engine.WriteToFile(options, people, "07-mixing-global-and-property-renderers.csv");
    }

    private record RowData(DateOnly DateWithGlobalCustomFormat, DateOnly DateWithPropertyCustomFormat, DateOnly DateWithAnotherPropertyCustomFormat);

    public class DateOnlyRenderer : QuotableFieldRenderer<DateOnly> {
        private readonly string _format;

        public DateOnlyRenderer(string format = "yyyy-MM-dd") {
            _format = format;
        }

        public DateOnlyRenderer() : this("yyyy-MM-dd") {
        }

        protected override string RenderValue(CsvWritingContext context, DateOnly value) {
            return value.ToString(_format);
        }
    }
}
