using CsvMagic.Reading;
using Samples.ReadCsv;

try {
    await GettingStartedProgram.Execute();
}
catch (CsvReadingException ex) {
    Console.WriteLine(ex.LineNumber);
    Console.WriteLine(ex.LineText);
    Console.WriteLine(ex.ErrorDetail);
    Console.WriteLine(ex.ParserTag);
    Console.WriteLine(ex.TokenText);
}