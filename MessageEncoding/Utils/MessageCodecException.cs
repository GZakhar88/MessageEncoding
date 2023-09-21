namespace MessageEncoding.Utils;

public class MessageCodecException : Exception
{
    public MessageCodecException() : base() { }
    public MessageCodecException(string message) : base(message) { }
    public MessageCodecException(string message, Exception innerException) : base(message, innerException) { }
}