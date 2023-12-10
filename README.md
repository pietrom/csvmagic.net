# CsvMagic
CsvMagic is a free and easy to use .NET library to read/write data from delimited record in streams.

# .Net Support
Currently CsvMagic is based on .Net 7.0. In the future the library could be backported to previous version of the framework, but at the moment such a porting activity han not been planned.

# Badges
| Branch  | Pipeline Status                                                                                                                                | Code Coverage                                                                  |
|---------|:-----------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------|
| main    | [![pipeline status](https://gitlab.com/darwinsw/csvmagic.net/badges/main/pipeline.svg)](https://gitlab.com/darwinsw/csvmagic.net/commits/main) | [![coverage](https://gitlab.com/darwinsw/csvmagic.net/badges/main/coverage.svg)](https://darwinsw.gitlab.io/csvmagic.net/coverage/main/html-report/index.htm) |

# Installation
CsvMagic is distributed as *[nuget package](https://www.nuget.org/packages/CsvMagic)*: it can be added to a project simply launching
```bash
dotnet add package CsvMagic
```
in the command line, or using your preferred IDE's packages management facility.

# Usage
## Writing CSV streams
### Getting Started
Basically, you should get an instance of `CsvWritingEngine<T>` and pass an `IEnumerable<T>` to its `Write` method.
In order to call `Write` you need a `StreamWriter` (handling the effective writing) and an instance of `CsvOptions` (controlling writing preferences).

Here is the code:
```csharp
public class SampleRow
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirtdDay { get; set; }
}
```
```csharp
using CsvMagic;
using CsvMagic.Writing;
using Samples.WriteCsv;

var engine = new CsvWritingEngineFactory().Create<SampleRow>();

var rows = new[] {
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

var options = CsvOptions.Default();

await engine.Write(rows, new StreamWriter(File.OpenWrite("cyclists.csv")), options);
```
And here the result:
```csv
Id,FirstName,LastName,BirtdDay,Ranking
1,Eddy,Merckx,1947-06-17,100
2,"Bernard ""Le Blaireau""",Hinault,1954-11-14,95.7
100,Miguel,Indurain,1964-07-16,75.2
501,Alberto,Contador,1982-12-06,81.3

```
### Options
CsvMagic allows the customization of some aspects of the rendering process:
- you can choose a *field delimiter* other than `,`
- you can choose a *quoting character* other than `"`
- you can omit the first line, containing field names
- you can choose a *decimal separator* other than `.`

Configuration options can be specified providing a `CsvOptions` different other than `CsvOptions.Default()`: using
```csharp
var options = CsvOptions.Builder()
    .WithoutHeaders()
    .WithDelimiter(';')
    .WithQuoting('\'')
    .WithDecimalSeparator(',')
    .Build();
```
you can generate a CSV file having the following content:
```csv
1;Eddy;Merckx;1947-06-17;100
2;Bernard "Le Blaireau";Hinault;1954-11-14;95,7
100;Miguel;Indurain;1964-07-16;75,2
501;Alberto;Contador;1982-12-06;81,3

```
(`Bernard "Le Blaireau"` needs now no quoting because it doesn't contains the chosen *quoting character*).
### DTOs and readable properties
CsvMagic includes in the generated CSV stream all public gettable properties exposed by the class for which the `CsvWritingEngine`'s class has been created and from its superclasses.
So, if you add a *read-only* property, e.g. `public string FullName => $"{FirstName} {LastName}";"`, you find a new field in the produced CSV:
```csv
Id,FirstName,LastName,BirtdDay,Ranking,FullName
1,Eddy,Merckx,1947-06-17,100,Eddy Merckx
2,"Bernard ""Le Blaireau""",Hinault,1954-11-14,95.7,"Bernard ""Le Blaireau"" Hinault"
100,Miguel,Indurain,1964-07-16,75.2,Miguel Indurain
501,Alberto,Contador,1982-12-06,81.3,Alberto Contador

```

### Recursive rendering
CsvMagic renders complex properties recursively, producing CSV records containing properties of nested object:
Say you have the following classes:
```csharp
public class Pet {
    public string Name { get; set; }
    public Person Owner { get; set; }
    public string Color { get; set; }
}

public class Person {
    public string FullName { get; set; }
    public Address Address { get; set; }
    public int Age { get; set; }
}

public class Address {
    public string Street { get; set; }
    public string Number { get; set; }
}
```
Rendering the following `rows`
```csharp
var rows = new[] {
    new Pet {
        Name = "Snow",
        Owner = new Person {
            FullName = "Pietro Martinelli", Address = new Address { Street = "Main Street", Number = "19/C" },
            Age = 45
        },
        Color = "Black"
    },
    new Pet {
        Name = "Birba",
        Owner = new Person {
            FullName = "Gargamella", Address = new Address { Street = "Wood Street", Number = "11" }, Age = 115
        },
        Color = "Brown"
    },
};
```
produces the following CSV content:
```csv
Name,FullName,Street,Number,Age,Color
Snow,Pietro Martinelli,Main Street,19/C,45,Black
Birba,Gargamella,Wood Street,11,115,Brown

```
Thanks to #29, you can obtain fully-qualified property names in header row, too: rendering the same dataset as in the preview example with
```
var options = CsvOptions.Builder()
    .FullyQualifyNestedProperties()
    .Build()
```
produces the following CSV content:
```csv
Name,Owner_FullName,Owner_Address_Street,Owner_Address_Number,Owner_Age,Color
Snow,Pietro Martinelli,Main Street,19/C,45,Black
Birba,Gargamella,Wood Street,11,115,Brown

```
in case of nested, complex objects.

### Default renderers and custom renderers
CsvMagic gives you the ability to control the way a property is rendered through the `interface FieldRenderer`.
CsvMagic chooses an implementation of `FieldRenderer` and uses it to render the value of a property.
The choice is made according to the following steps:
- default rendered are provided *out of the box* for common types (and their *nullable* counterparts):
  - int
  - long
  - decimal
  - double
  - int
  - DateOnly
  - DateTimeOffset
  - bool
  - string
- invoking `CsvWritingEngineFactory.RegisterRenderer<T>(FieldRenderer)` you can force CsvMagic to use your own implementation of `FieldRender` every time a property of type `T` should be rendered
- decorating a specific property with the `[CsvField(Renderer = typeof(CustomRenderer))]` attribute you can force CsvMagic to use your own implementation of `FieldRender` every time that property should be rendered
## Reading CSV streams
### Getting started
Basically, you should get an instance of `CsvReadingEngine<T>` and pass a `StreamReader` to its `Read` method.
In order to call `Write` you need an instance of `CsvOptions` (controlling reading preferences).

Here is the code:
```csharp
public record SampleRow { // records have 
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirtdDay { get; set; }
    public decimal Ranking { get; set; }
}
```
```csharp
var engine = new CsvReadingEngineFactory().Create<SampleRow>();

        await File.WriteAllTextAsync("cyclists.csv", @"Id,FirstName,LastName,BirtdDay,Ranking,FullName
2,""Bernard """"Le Blaireau"""""",Hinault,1954-11-14,95.7
100,Miguel,Indurain,1964-07-16,75.2
");

        var rows = await engine.Read(CsvOptions.Default(), new StreamReader(File.OpenRead("cyclists.csv"))).ToListAsync();

        foreach (var row in rows) {
            Console.WriteLine(row);
        }
    }
```
The produced output is
```
SampleRow { Id = 2, FirstName = Bernard "Le Blaireau", LastName = Hinault, BirtdDay = 11/14/1954, Ranking = 95.7 }
SampleRow { Id = 100, FirstName = Miguel, LastName = Indurain, BirtdDay = 7/16/1964, Ranking = 75.2 }
```
### DTOs and writable properties
CsvMagic reads from CSV stream all public settable properties exposed by the class for which the `CsvReadingEngine`'s class has been created and from its superclasses.

Try for example to define a type containing a *set-only* property, like `FullName` in the following code-snippet:
```csharp
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
```
Now you can provide a CSV like
```csv
@"Id,FullName
19,Pietro Martinelli
"
```
and deserialize it with
```csharp
var engine = new CsvReadingEngineFactory().Create<SampleRowContainingSetOnlyProperty>();

await File.WriteAllTextAsync("cyclists.csv", @"Id,FullName
19,Pietro Martinelli
");

var row = await engine.Read(CsvOptions.Default(), new StreamReader(File.OpenRead("cyclists.csv"))).SingleAsync();
Console.WriteLine(row);
```
obtaining the following output:
```
SampleRowContainingSetOnlyProperty { Id = 19, FirstName = Pietro, LastName = Martinelli }
```
So, CsvMagic finds two fields in the input text `19,Pietro Martinelli`, and two *settable* properties in the target type, and invokes their setters.
### Options
CsvMagic supports in reading same config options supported in writing: see  *Writing CSV streams / Options* for full explanation

### Recursive parsing

### Implementing custom parsers

### Rows containing more fields than needed

### Rows containing less fields than needed

## The CSV format: quoting field values