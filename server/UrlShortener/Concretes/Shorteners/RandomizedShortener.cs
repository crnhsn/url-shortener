using System.Text;
using UrlShortener.Concretes.Encoding;
using UrlShortener.Interfaces;

namespace UrlShortener.Concretes.Shorteners;

public class RandomizedHashBasedShortener : IShorteningProvider<string, string> {
    
    private readonly int _maxLength;
    
    private readonly IHashProvider<byte[], string> _hashProvider;
    private readonly IEncoder<byte[], string> _encoder;
    
    private readonly IRandomnessProvider<string> _randomnessProvider;
    
    public RandomizedHashBasedShortener(int maxLength,
                                        IHashProvider<byte[], string> hashProvider,
                                        IEncoder<byte[], string> encoder,
                                        IRandomnessProvider<string> randomnessProvider)
    {
        _maxLength = maxLength;
        _hashProvider = hashProvider;
        _encoder = encoder;
        _randomnessProvider = randomnessProvider;
    }
    
    public string Shorten(string toShorten)
    {
       
        string randomValue = _randomnessProvider.GenerateRandomValue();
        string randomizedString = toShorten + randomValue;
        
        byte[] hash = _hashProvider.ComputeHash(randomizedString);
        
        string encoded = _encoder.Encode(hash);
        
        if (encoded.Length == 0)
        {
            throw new Exception("Encoding generated a string of length 0.");
        }

        if (encoded.Length < _maxLength)
        {
            encoded = padString(encoded, _maxLength);
        }

        string truncated = encoded.Substring(0, _maxLength);

        return truncated;
    }

    private string padString(string toPad, int padLength)
    {
        var sb = new StringBuilder(toPad);
        while (sb.Length < padLength)
        {
            string randomValue = _randomnessProvider.GenerateRandomValue();
            sb.Append(randomValue);
        }

        foreach (char unsafeChar in UrlSafetyConstants.URL_UNSAFE_CHARACTERS)
        {
            sb.Replace(unsafeChar.ToString(), "");
        }

        if (sb.Length < padLength)
        {
            throw new Exception("Encoded string was below max length. Tried to pad, but padding failed to sufficiently lengthen string.");
        }

        return sb.ToString();
    }
    
}