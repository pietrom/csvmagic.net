namespace Samples.ReadCsv;

public record SampleRowContainingSetOnlyProperty {
    public int Id { get; set; }
    private string firstName = string.Empty;
    public string FirstName => firstName;
    private string lastName = string.Empty;
    public string LastName => lastName;

    public string FullName {
        set {
            var fields = value.Split(' ');
            firstName = fields[0];
            lastName = fields[1];
        }
    }
}