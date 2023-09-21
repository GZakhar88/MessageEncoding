using MessageEncoding.Utils;

namespace MessageEncoding.Tests;

using System.Collections.Generic;
using MessageEncoding;
using Xunit;

public class MessageValidatorTests
{
    [Fact]
    public void TestMessageValidation_ValidMessage_ReturnTrue()
    {
        //Arrange
        var validator = new MessageValidator();
        var message = new Message
        {
            Headers = new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            },
            Payload = "This is the payload."u8.ToArray()
        };

        //Act
        bool isValid = validator.ValidateMessage(message);

        //Assert
        Assert.True(isValid);
    }

    [Fact]
    public void TestMessageValidation_NullHeaders_ReturnFalse()
    {
        //Arrange
        var validator = new MessageValidator();
        var message = new Message
        {
            Headers = null,
            Payload = "This is the payload."u8.ToArray()
        };

        //Act
        bool isValid = validator.ValidateMessage(message);

        //Assert
        Assert.False(isValid);
    }

    [Fact]
    public void TestMessageValidation_NullPayload_ReturnFalse()
    {
        //Arrange
        var validator = new MessageValidator();
        var message = new Message
        {
            Headers = new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            },
            Payload = null
        };

        //Act
        bool isValid = validator.ValidateMessage(message);

        //Assert
        Assert.False(isValid);
    }

    [Fact]
    public void TestMessageValidation_InvalidPayloadSize_ReturnFalse()
    {
        //Arrange
        var validator = new MessageValidator();
        var message = new Message
        {
            Headers = new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            },
            Payload = new byte[256 * 1024 + 1]
        };

        //Act
        bool isValid = validator.ValidateMessage(message);

        //Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void TestMessageValidation_ValidPayloadSize_ReturnTrue()
    {
        // Arrange
        var validator = new MessageValidator();
        var message = new Message
        {
            Headers = new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            },
            Payload = new byte[256 * 1024]
        };

        // Act
        bool isValid = validator.ValidateMessage(message);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void TestMessageValidation_InvalidHeaderLengths_ReturnFalse()
    {
        //Arrange
        var validator = new MessageValidator();
        var message = new Message
        {
            Headers = new Dictionary<string, string>
            {
                { new string('A', 1024), "Value1" }
            },
            Payload = "This is the payload."u8.ToArray()
        };

        //Act
        bool isValid = validator.ValidateMessage(message);

        //Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void TestMessageValidation_ValidHeaderLengths_ReturnTrue()
    {
        // Arrange
        var validator = new MessageValidator();
        var message = new Message
        {
            Headers = new Dictionary<string, string>
            {
                { new string('A', 1023), new string('B', 1023) }
            },
            Payload = "This is the payload."u8.ToArray()
        };

        // Act
        bool isValid = validator.ValidateMessage(message);

        // Assert
        Assert.True(isValid);
    }
}
