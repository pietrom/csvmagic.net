﻿using CsvMagic;

namespace CsvMagicTests.Reading;

public record CsvReadData : Data {
    public string? StringValue { get; set; }
    public long LongValue { get; set; }
    public DateOnly DefaultDateOnly { get; set; }
    public DateOnly CustomDateOnly { get; set; }
    public DateOnly? DefaultNullableDateOnly { get; set; }
}

public record CsvReadDataPoco : Data {
    public string? StringValue { get; set; }
    public long LongValue { get; set; }
    public DateOnly DefaultDateOnly { get; set; }
    public DateOnly CustomDateOnly { get; set; }
    public DateOnly? DefaultNullableDateOnly { get; set; }
}
