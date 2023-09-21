using MessageEncoding.Utils;

namespace MessageEncoding;

internal static class Program
{
    private static void Main(string[] args)
    {
        //Example usage
        ICodec codec = new MessageCodec();
        
        var message = new Message
        { 
            Headers = new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            },
            Payload = "This is the message payload."u8.ToArray()
        };
        
        try
        {
            //Invoke encoding
            byte[] encodedData = codec.Encode(message);
            
            //Invoke decoding
            Message decodedMessage = codec.Decode(encodedData);

            //Display result
            Display.Print(encodedData, decodedMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}