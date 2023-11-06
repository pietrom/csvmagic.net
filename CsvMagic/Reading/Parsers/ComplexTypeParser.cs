namespace CsvMagic.Reading.Parsers;

public abstract class ComplexTypeParser<T> : FieldParser
{
    public (object?, string?) ParseNext(CsvOptions options, string? text)
    {
        var (parameters, rest) = SubParsers.Aggregate(
            (Parameters: new List<object?>() as IEnumerable<object?>, Text: text), (acc, curr) =>
            {
                var (o, r) = curr.ParseNext(options, acc.Text);
                return (acc.Parameters.Append(o), r);
            });
        return (Build(parameters.ToArray()), rest);
    }

    protected abstract IList<FieldParser> SubParsers { get; }

    protected abstract T? Build(object?[] parameters);
}