using CsvMagic;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

public static class CsvWritingContextHelper {
    public static CsvWritingContext ContextFrom(CsvOptions options) => new CsvWritingContext(options, new Dictionary<Type, FieldRenderer>());
}
