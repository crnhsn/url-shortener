using UrlShortener.Interfaces;

namespace UrlShortener.Concretes.Encoding;

public class UrlSafeBase64Encoder : IEncoder<byte[], string> {
    
    private static readonly List<char> URL_UNSAFE_CHARACTERS = new List<char>() {'+', '=', '/'};
    
    public UrlSafeBase64Encoder()
    {
        
    }
    
    public string Encode(byte[] input)
    {
        string encoded = urlSafeBase64Encode(input);
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
    
}