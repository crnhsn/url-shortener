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
        
        return encoded;
    }
    
}