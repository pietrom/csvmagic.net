using CsvMagic;
using CsvMagic.Writing;
using Microsoft.AspNetCore.Mvc;

namespace Samples.StreamingCsvFromApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamingCsvController : ControllerBase {
    [HttpGet(Name = "GetCsv")]
    public async Task<IActionResult> Get() {
        var cyclists = new[] {
            new SampleRow {
                Id = 1, FirstName = "Eddy", LastName = "Merckx", BirtdDay = new DateOnly(1947, 6, 17), Ranking = 100
            },
            new SampleRow {
                Id = 2, FirstName = "Bernard \"Le Blaireau\"", LastName = "Hinault",
                BirtdDay = new DateOnly(1954, 11, 14), Ranking = 95.7m
            },
            new SampleRow {
                Id = 100, FirstName = "Miguel", LastName = "Indurain", BirtdDay = new DateOnly(1964, 7, 16),
                Ranking = 75.2m
            },
            new SampleRow {
                Id = 501, FirstName = "Alberto", LastName = "Contador", BirtdDay = new DateOnly(1982, 12, 6),
                Ranking = 81.3m
            }
        };

        var stream = new MemoryStream();
        var engine = new CsvWritingEngineFactory().Create<SampleRow>();
        await engine.Write(CsvOptions.Default(), cyclists, new StreamWriter(stream));
        stream.Seek(0, SeekOrigin.Begin);
        return File(stream, "text/csv", "cyclists.csv");
    }
}

public class SampleRow {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirtdDay { get; set; }
    public decimal Ranking { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
