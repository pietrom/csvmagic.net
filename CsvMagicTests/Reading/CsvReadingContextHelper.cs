using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

public static class CsvReadingContextHelper
{
    public static CsvReadingContext ContextFrom(CsvOptions options) => new CsvReadingContext(options, new Dictionary<Type, FieldParser>());
}