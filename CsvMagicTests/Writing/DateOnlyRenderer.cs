using CsvMagic.Writing;
using CsvMagic.Writing.Renderers;

namespace CsvMagicTests.Writing;

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
