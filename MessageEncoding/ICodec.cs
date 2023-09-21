namespace MessageEncoding;

public interface ICodec
{
    byte[] Encode(Message message);
    Message Decode(byte[] data);
}