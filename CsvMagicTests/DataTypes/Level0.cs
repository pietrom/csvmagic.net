namespace CsvMagicTests.DataTypes;

public record Level0 {
    public string Field00 { get; set; }
    public Level1 Field01 { get; set; }
    public int Field02 { get; set; }
}

public record Level1 {
    public int Field10 { get; set; }
    public Level2 Field11 { get; set; }
    public DateOnly Field12 { get; set; }
}

public record Level2 {
    public string Field20 { get; set; }
    public DateOnly Field21 { get; set; }
    public string Field22 { get; set; }
}

