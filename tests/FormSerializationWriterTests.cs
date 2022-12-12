using System.Text;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Serialization.Form.Tests.Mocks;

namespace Microsoft.Kiota.Serialization.Form.Tests;
public class FormSerializationWriterTests
{
    [Fact]
    public void WritesSampleObjectValue()
    {
        // Arrange
        var testEntity = new TestEntity()
        {
            Id = "48d31887-5fad-4d73-a9f5-3c356e68a038",
            WorkDuration = TimeSpan.FromHours(1),
            StartWorkTime = new Time(8, 0, 0),
            BirthDay = new Date(2017, 9, 4),
            AdditionalData = new Dictionary<string, object>
            {
                {"mobilePhone", null!}, // write null value
                {"accountEnabled", false}, // write bool value
                {"jobTitle","Author"}, // write string value
                {"createdDateTime", DateTimeOffset.MinValue}, // write date value
            }
        };
        using var formSerializerWriter = new FormSerializationWriter();
        // Act
        formSerializerWriter.WriteObjectValue(string.Empty,testEntity);
        // Get the string from the stream.
        var serializedStream = formSerializerWriter.GetSerializedContent();
        using var reader = new StreamReader(serializedStream, Encoding.UTF8);
        var serializedFormString = reader.ReadToEnd();
        
        // Assert
        var expectedString =    "id=48d31887-5fad-4d73-a9f5-3c356e68a038&" +
                                "workDuration=PT1H&"+    // Serializes timespans
                                "birthDay=2017-09-04&" + // Serializes dates
                                "startWorkTime=08%3A00%3A00&" + //Serializes times
                                "mobilePhone=null&" + // Serializes null values
                                "accountEnabled=false&" +
                                "jobTitle=Author&" +
                                "createdDateTime=0001-01-01T00%3A00%3A00.0000000%2B00%3A00";
        Assert.Equal(expectedString, serializedFormString);
    }

    [Fact]
    public void WritesSampleCollectionOfObjectValues()
    {
        // Arrange
        var testEntity = new TestEntity()
        {
            Id = "48d31887-5fad-4d73-a9f5-3c356e68a038",
            Numbers = TestEnum.One | TestEnum.Two,
            AdditionalData = new Dictionary<string, object>
            {
                {"mobilePhone",null!}, // write null value
                {"accountEnabled",false}, // write bool value
                {"jobTitle","Author"}, // write string value
                {"createdDateTime", DateTimeOffset.MinValue}, // write date value
            }
        };
        var entityList = new List<TestEntity>() { testEntity};
        using var formSerializerWriter = new FormSerializationWriter();
        // Act
        Assert.Throws<InvalidOperationException>(() => formSerializerWriter.WriteCollectionOfObjectValues(string.Empty, entityList));
    }

}
