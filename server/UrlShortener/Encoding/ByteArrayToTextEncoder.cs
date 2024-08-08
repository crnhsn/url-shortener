using UrlShortener.Interfaces;

namespace UrlShortener.Encoding;

public class ByteArrayToTextEncoder : IEncoder<byte[], string> {
    
    private static readonly List<char> URL_UNSAFE_CHARACTERS = new List<char>() {'+', '=', '/'};
    
    private readonly TextEncodingType _encodingType;
    
    public ByteArrayToTextEncoder(TextEncodingType encodingType)
    {
        _encodingType = encodingType;
    }
    
    public string Encode(byte[] input)
    {
        string encoded;
        
        switch(_encodingType) 
        {
          case TextEncodingType.Base32:
            encoded = base32Encode(input);
            break;
          
          case TextEncodingType.UrlSafeBase64:
            encoded = urlSafeBase64Encode(input);
            break;
            
          default:
            encoded = urlSafeBase64Encode(input);
            break;
        }
        
        return encoded;
        
    }
    
    private static string urlSafeBase64Encode(byte[] input)
    {
        string base64String = Convert.ToBase64String(input);
        
        foreach (char unsafeChar in URL_UNSAFE_CHARACTERS)
        {
            base64String = base64String.Replace(unsafeChar.ToString(), "");
        }
        
        return base64String;
        
    }
    
    private static string base32Encode(byte[] input)
    {
        throw new NotImplementedException();
    }

    
}