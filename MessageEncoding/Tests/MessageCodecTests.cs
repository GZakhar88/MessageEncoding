using MessageEncoding.Utils;
using Xunit;

namespace MessageEncoding.Tests;

public class MessageCodecTests
{
    [Fact]
    public void TestMessageEncodingAndDecoding_WithValidMessage_ShouldEqual()
    {
        //Arrange
        var originalMessage = new Message
        {
            Headers = new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            },
            Payload = "This is the payload."u8.ToArray()
        };

        //Act
        ICodec codec = new MessageCodec();
        byte[] encodedData = codec.Encode(originalMessage)!;
        Message decodedMessage = codec.Decode(encodedData)!;

        
        //Assert
        Assert.Equal(originalMessage.Headers.Count, decodedMessage.Headers!.Count);

        foreach (var header in originalMessage.Headers)
        {
            Assert.True(decodedMessage.Headers.ContainsKey(header.Key));
            Assert.Equal(originalMessage.Headers[header.Key], decodedMessage.Headers[header.Key]);
        }

        Assert.Equal(originalMessage.Payload, decodedMessage.Payload);
    }
    
    [Fact]
    public void TestMessageEncoding_HeaderExceedingLength_ThrowException()
    {
        //Arrange
        var message = new Message
        {
            Headers = new Dictionary<string, string>
            {
                { new string('A', 1024), "Value1" }
            },
            Payload = "This is the payload."u8.ToArray()
        };
        
        ICodec codec = new MessageCodec();
        
        //Act and Assert
        Assert.Throws<MessageCodecException>(() => codec.Encode(message));
    }
    
    [Fact]
    public void TestMessageEncoding_PayloadExceedingSize_ThrowException()
    {
        //Arrange
        var message = new Message
        {
            Headers = new Dictionary<string, string>(),
            Payload = new byte[256 * 1024 + 1]
        };
        
        ICodec codec = new MessageCodec();

        //Act and Assert
        Assert.Throws<MessageCodecException>(() => codec.Encode(message));
    }
    
    
    
    [Fact]
    public void TestMessageEncoding_NullMessage_ThrowException()
    {
        //Arrange
        var nullMessage = new Message();
        ICodec codec = new MessageCodec();

        //Act and Assert
        Assert.Throws<MessageCodecException>(() => codec.Encode(nullMessage));
    }
    
    [Fact]
    public void TestMessageEncoding_NullHeaders_ThrowException()
    {
        //Arrange
        var message = new Message
        {
            Headers = null,
            Payload = "This is the payload."u8.ToArray()
        };

        ICodec codec = new MessageCodec();
        
        //Act and Assert
        Assert.Throws<MessageCodecException>(() => codec.Encode(message));
    }
    
}