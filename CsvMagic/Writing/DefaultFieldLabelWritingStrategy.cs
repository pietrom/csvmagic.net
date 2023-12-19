using System.Reflection;

namespace CsvMagic.Writing;

public class DefaultFieldLabelWritingStrategy : FieldLabelWritingStrategy {
    public string GetLabel(PropertyInfo info) {
        return info.Name;
    }
}
