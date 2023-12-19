using System.Reflection;

namespace CsvMagic.Writing;

public interface FieldLabelWritingStrategy {
    string GetLabel(PropertyInfo info);
}
