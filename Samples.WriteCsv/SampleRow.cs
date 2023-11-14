namespace Samples.WriteCsv;

public class SampleRow {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirtdDay { get; set; }
    public decimal Ranking { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
