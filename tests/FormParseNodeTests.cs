using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Serialization.Form.Tests.Mocks;

namespace Microsoft.Kiota.Serialization.Form.Tests;
public class FormParseNodeTests
{
    private const string TestUserForm = "displayName=Megan+Bowen&" +
                                        "numbers=one,two,thirtytwo&" +
                                        "givenName=Megan&" +
                                        "accountEnabled=true&" +
                                        "createdDateTime=2017-07-29T03:07:25Z&" +
                                        "jobTitle=Auditor&" +
                                        "mail=MeganB@M365x214355.onmicrosoft.com&" +
                                        "mobilePhone=null&" +
                                        "officeLocation=null&" +
                                        "preferredLanguage=en-US&" +
                                        "surname=Bowen&" +
                                        "workDuration=PT1H&" +
                                        "startWorkTime=08:00:00.0000000&" +
                                        "endWorkTime=17:00:00.0000000&" +
                                        "userPrincipalName=MeganB@M365x214355.onmicrosoft.com&" +
                                        "birthDay=2017-09-04&" +
                                        "id=48d31887-5fad-4d73-a9f5-3c356e68a038";

    [Fact]
    public void GetsEntityValueFromForm()
    {
        // Arrange
        var formParseNode = new FormParseNode(TestUserForm);
        // Act
        var testEntity = formParseNode.GetObjectValue<TestEntity>(x => new TestEntity());
        // Assert
        Assert.NotNull(testEntity);
        Assert.Null(testEntity.OfficeLocation);
        Assert.NotEmpty(testEntity.AdditionalData);
        Assert.True(testEntity.AdditionalData.ContainsKey("jobTitle"));
        Assert.True(testEntity.AdditionalData.ContainsKey("mobilePhone"));
        Assert.Equal("Auditor", testEntity.AdditionalData["jobTitle"]);
        Assert.Equal("48d31887-5fad-4d73-a9f5-3c356e68a038", testEntity.Id);
        Assert.Equal(TestEnum.One | TestEnum.Two, testEntity.Numbers ); // Unknown enum value is not included
        Assert.Equal(TimeSpan.FromHours(1), testEntity.WorkDuration); // Parses timespan values
        Assert.Equal(new Time(8,0,0).ToString(),testEntity.StartWorkTime.ToString());// Parses time values
        Assert.Equal(new Time(17, 0, 0).ToString(), testEntity.EndWorkTime.ToString());// Parses time values
        Assert.Equal(new Date(2017,9,4).ToString(), testEntity.BirthDay.ToString());// Parses date values
    }

    [Fact]
    public void GetCollectionOfObjectValuesFromForm()
    {
        var formParseNode = new FormParseNode(TestUserForm);
        Assert.Throws<InvalidOperationException>(() => formParseNode.GetCollectionOfObjectValues<TestEntity>(static x => new TestEntity()));
    }

    [Fact]
    public void GetsChildNodeAndGetCollectionOfPrimitiveValuesFromFormParseNode()
    {
        var rootParseNode = new FormParseNode(TestUserForm);
        Assert.Throws<InvalidOperationException>(() => rootParseNode.GetCollectionOfPrimitiveValues<string>());
    }

    [Fact]
    public void ReturnsDefaultIfChildNodeDoesNotExist()
    {
        // Arrange
        var rootParseNode = new FormParseNode(TestUserForm);
        // Try to get an imaginary node value
        var imaginaryNode = rootParseNode.GetChildNode("imaginaryNode");
        // Assert
        Assert.Null(imaginaryNode);
    }
}
