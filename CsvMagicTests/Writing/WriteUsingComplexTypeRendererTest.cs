﻿using CsvMagic;
using CsvMagic.Writing;
using CsvMagicTests.DataTypes;

namespace CsvMagicTests.Writing;

[TestFixture]
public class WriteUsingComplexTypeRendererTest {
    private static readonly Level0[] Data = new[] {
        new Level0 {
            Field00 = "F00", Field02 = 19, Field01 = new Level1 {
                Field10 = 11,
                Field12 = new DateOnly(1978, 11, 11),
                Field11 = new Level2 {
                    Field20 = "Aaa",
                    Field21 = new DateOnly(1978, 3, 19),
                    Field22 = "Bbb"
                }
            }
        }
    };

    private static readonly Level0WithCustomLabels[] DataWithCustomLabels = new[] {
        new Level0WithCustomLabels {
            Field00 = "F00", Field02 = 19, Field01 = new Level1WithCustomLabels {
                Field10 = 11,
                Field12 = new DateOnly(1978, 11, 11),
                Field11 = new Level2WithCustomLabels {
                    Field20 = "Aaa",
                    Field21 = new DateOnly(1978, 3, 19),
                    Field22 = "Bbb"
                }
            }
        }
    };

    [Test]
    public async Task ShouldNotWriteSetterOnlyProperty() {
        var engine = new CsvWritingEngineFactory().Create<Level0>();
        var result = await engine.WriteToString(Data);
        Assert.That(result, Is.EqualTo(@"Field00,Field10,Field20,Field21,Field22,Field12,Field02
F00,11,Aaa,1978-03-19,Bbb,1978-11-11,19
"));
    }

    [Test]
    public async Task ShouldWriteFullyQualifiedNestedPropertyNames() {
        var engine = new CsvWritingEngineFactory().Create<Level0>();
        var result = await engine.WriteToString(CsvOptions.Builder().FullyQualifyNestedProperties().Build(), Data);
        Assert.That(result, Is.EqualTo(@"Field00,Field01_Field10,Field01_Field11_Field20,Field01_Field11_Field21,Field01_Field11_Field22,Field01_Field12,Field02
F00,11,Aaa,1978-03-19,Bbb,1978-11-11,19
"));
    }


    [Test]
    public async Task ShouldWriteFullyQualifiedNestedPropertyNamesWithCustomLabels() {
        var engine = new CsvWritingEngineFactory().Create<Level0WithCustomLabels>();
        var result = await engine.WriteToString(CsvOptions.Builder().FullyQualifyNestedProperties().Build(), DataWithCustomLabels);
        Assert.That(result, Is.EqualTo(@"Field00,Nested0_Int10,Nested0_Nested1_Field20,Nested0_Nested1_Field21,Nested0_Nested1_Text22,Nested0_Field12,Field02
F00,11,Aaa,1978-03-19,Bbb,1978-11-11,19
"));
    }
}
