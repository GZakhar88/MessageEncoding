using System.Text;
using MessageEncoding.Utils;

namespace MessageEncoding;

public class MessageCodec : ICodec
{
    /// <summary>
    /// Encodes a message into a binary format according to the specified encoding scheme.
    /// </summary>
    /// <param name="message">The message to be encoded.</param>
    /// <returns>The binary representation of the encoded message.</returns>
    /// <exception cref="MessageCodecException">Thrown when the message is invalid and cannot be encoded.</exception>
    public byte[] Encode(Message message)
    {
        var validator = new MessageValidator(); 
        if (!validator.ValidateMessage(message))
        {
            throw new MessageCodecException("Invalid message.");
        }

        byte[] headerCountBytes = BitConverter.GetBytes((ushort)message.Headers!.Count);
        byte[] payloadSizeBytes = BitConverter.GetBytes(message.Payload!.Length);

        var encodedMessage = new List<byte>();
        encodedMessage.AddRange(headerCountBytes);
        encodedMessage.AddRange(payloadSizeBytes);
        
        foreach (var header in message.Headers)
        {
            byte[] headerNameLengthBytes = BitConverter.GetBytes((ushort)header.Key.Length);
            byte[] headerNameBytes = Encoding.ASCII.GetBytes(header.Key);
            
            byte[] headerValueLengthBytes = BitConverter.GetBytes((ushort)header.Value.Length);
            byte[] headerValueBytes = Encoding.ASCII.GetBytes(header.Value);

            encodedMessage.AddRange(headerNameLengthBytes);
            encodedMessage.AddRange(headerNameBytes);
            
            encodedMessage.AddRange(headerValueLengthBytes);
            encodedMessage.AddRange(headerValueBytes);
        }
        
        encodedMessage.AddRange(message.Payload);

        return encodedMessage.ToArray();
    }
    
    /// <summary>
    /// Decodes a binary message into a <see cref="Message"/> based on the specified encoding scheme.
    /// </summary>
    /// <param name="data">The binary data to be decoded into a message.</param>
    /// <returns>The decoded <see cref="Message"/>.</returns>
    /// <exception cref="MessageCodecException">Thrown when the data is invalid and cannot be decoded.</exception>
    public Message Decode(byte[] data)
    {
        if (data is null || data.Length < 4)
        {
            throw new MessageCodecException("Invalid data to decode.");
        }
        
        var currentIndex = 0;
        
        ushort headerCount = BitConverter.ToUInt16(data, currentIndex);
        if (headerCount > 63)
        {
            throw new MessageCodecException("Invalid header count in the encoded message.");
        }
        
        currentIndex += 2;

        int payloadSize = BitConverter.ToInt32(data, currentIndex);
        if (payloadSize > 256 * 1024)
        {
            throw new MessageCodecException("Invalid payload size in the encoded message.");
        }
        
        currentIndex += 4;

        
        var headers = new Dictionary<string, string>();
        
        for (var i = 0; i < headerCount; i++)
        {
            ushort headerNameLength = BitConverter.ToUInt16(data, currentIndex);
            currentIndex += 2;

            string headerName = Encoding.ASCII.GetString(data, currentIndex, headerNameLength);
            currentIndex += headerNameLength;

            ushort headerValueLength = BitConverter.ToUInt16(data, currentIndex);
            currentIndex += 2;

            string headerValue = Encoding.ASCII.GetString(data, currentIndex, headerValueLength);
            currentIndex += headerValueLength;

            headers[headerName] = headerValue;
        }
        
        var payload = new byte[payloadSize];
        Buffer.BlockCopy(data, currentIndex, payload, 0, payloadSize);

        return new Message { Headers = headers, Payload = payload };
    }
}