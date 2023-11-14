using CsvMagic.Reading;
using CsvMagic.Writing;

namespace CsvMagic;

public interface FieldSerializer : FieldRenderer, FieldParser {
}
