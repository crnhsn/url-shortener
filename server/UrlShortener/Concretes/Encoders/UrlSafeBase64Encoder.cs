using UrlShortener.Constants;
using UrlShortener.Interfaces;

namespace UrlShortener.Concretes.Encoders;

public class UrlSafeBase64Encoder : IEncoder<byte[], string> {
    
    public string Encode(byte[] input)
    {
        string encoded = urlSafeBase64Encode(input);
        return encoded;
        
    }
    
    private static string urlSafeBase64Encode(byte[] input)
    {
        string base64String = Convert.ToBase64String(input);
        
        foreach (char unsafeChar in UrlSafety.URL_UNSAFE_CHARACTERS)
        {
            base64String = base64String.Replace(unsafeChar.ToString(), "");
        }
        
        return base64String;
        
    }
    
}