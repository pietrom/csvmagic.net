namespace CsvMagic;

[AttributeUsage(AttributeTargets.Property)]
public class CsvField : Attribute {
    public Type? Renderer { get; set; }
    public Type? Parser { get; set; }
    public string? Label { get; set; }

    public Type? Serializer {
        set => Renderer = Parser = value;
    }
}
