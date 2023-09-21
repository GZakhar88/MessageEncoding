using System.Text;

namespace MessageEncoding.Utils;

public class MessageValidator
{
    public bool ValidateMessage(Message message)
    {
        return message is { Headers: not null, Payload: not null } 
               && IsPayloadSizeValid(message.Payload) 
               && ValidateHeaderLengths(message.Headers);
    }

    private static bool IsPayloadSizeValid(byte[] payload)
    {
        return payload.Length <= 256 * 1024;
    }

    private static bool ValidateHeaderLengths(Dictionary<string, string> headers)
    {
        return !headers.Any(header =>
            Encoding.ASCII.GetBytes(header.Key).Length > 1023 ||
            Encoding.ASCII.GetBytes(header.Value).Length > 1023);
    }
}