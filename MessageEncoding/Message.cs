namespace MessageEncoding;

public class Message
{
    public Dictionary<string, string>? Headers{ get; set; }
    public byte[]? Payload { get; set; }
}