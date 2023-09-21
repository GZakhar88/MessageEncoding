using System.Text;

namespace MessageEncoding.Utils;

public static class Display
{
    public static void Print(byte[] encodedData, Message decodedMessage)
    {
        Console.WriteLine("Encoded Message:");
        Console.WriteLine(BitConverter.ToString(encodedData));
        Console.WriteLine("========================================");
        Console.WriteLine("Decoded Message:");
        Console.WriteLine("Headers:");
        foreach (var header in decodedMessage.Headers!)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }

        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Payload: " + Encoding.ASCII.GetString(decodedMessage.Payload!));
        Console.WriteLine("========================================");
    }
}