# Message Encoder/Decoder

This project implements a simple binary message encoding and decoding codec. The encoding and decoding methods are designed for use in a signaling protocol for passing messages between peers in a real-time communication application.

## Assumptions

- A message can contain a variable number of headers and a binary payload.
- Header names and values are ASCII-encoded strings limited to **1023 bytes** each.
- A message can have a maximum of **63** headers.
- The message payload is limited to **256 KiB**.
- Maximum overall size of the message **≈382 KiB** </br>(maximum header count and payload size(6 bytes) + maximum headers size (129,150 bytes) + maximum payload size (262,144 bytes))

## Design Choices and Implementation

### Encoding Scheme

The encoding scheme is designed to efficiently represent the message structure in a compact binary format. The encoding follows these principles:

1. **Header Count and Payload Size**:</br> 
    We use 2 bytes for the header count (allowing up to 63 headers) and 4 bytes for the payload     size (supporting payloads up to 256 KiB).

2. **Headers Encoding**: Each header consists of:
   - 2 bytes for the header name **length**
   - Up to **1023 bytes** for the header name (ASCII-encoded)
   - 2 bytes for the header value **length**
   - Up to **1023 bytes** for the header value (ASCII-encoded)

3. **Payload**: 
    - The binary payload is included after the headers.
    - Up to **256KiB**

### Decoding Scheme

The decoding scheme reverses the encoding process, extracting the header count, payload size, headers, and the payload from the binary data.

### Error Handling

- Invalid data during decoding or encoding will throw a `MessageCodecException` to signify that the operation couldn't be completed due to invalid input data.
- Validation is done before encoding or decoding to ensure the message complies with the defined assumptions.

## Usage

To use the Message Encoder/Decoder, follow these steps:

1. **Initialize a Message Object**: Create a `Message` object with headers and a payload:
    ```csharp
    var message = new Message
    { 
        Headers = new Dictionary<string, string>
        {
            { "Header1", "Value1" },
            { "Header2", "Value2" }
        },
        Payload = "This is the message payload."u8.ToArray()
    };
    ```

2. **Encoding**: Use the `Encode` method to convert the `Message` object into a binary representation:
    ```csharp
    try
    {
        //Invoke encoding
        byte[] encodedData = codec.Encode(message);
    }
    catch (Exception ex)
    {
        //Handle exception
    }
    ```

3. **Decoding**: Use the `Decode` method to convert the binary data back into a `Message` object:
    ```csharp
    try
    {
        //Invoke decoding
        Message decodedMessage = codec.Decode(encodedData);
    }
    catch (Exception ex)
    {
        //Handle exception
    }
    ```

## Conclusion

The implementation of this message codec prioritizes simplicity and as a main goal avoid to use any third-party serializer. The encoding and decoding methods are suitable for use in a real-time communication application, striking a balance between compactness and readability.

---
