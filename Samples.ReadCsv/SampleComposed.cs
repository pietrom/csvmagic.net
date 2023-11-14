namespace Samples.ReadCsv;

public record Level0 {
    public int Value0 { get; set; }
    public Level1 Value1 { get; set; }
}

public record Level1 {
    public string Value10 { get; set; }
    public Level2 Value11 { get; set; }
    public string Value12 { get; set; }
}

public record Level2 {
    public int Value20 { get; set; }
    public DateOnly Value22 { get; set; }
}
