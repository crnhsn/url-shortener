using System.Security.Cryptography;
using UrlShortener.Constants;
using UrlShortener.Interfaces;

namespace UrlShortener.Concretes.Randomness;

public class UrlSafeRandomStringProvider : IRandomnessProvider<string>
{
    private readonly int _stringLength;
    private readonly string _alphabet = UrlSafety.URL_SAFE_ALPHABET;
    
    public UrlSafeRandomStringProvider(int stringLength)
    {
        _stringLength = stringLength;
    }
    
    public string GenerateRandomValue()
    {
        string randomString = RandomNumberGenerator.GetString(_alphabet, _stringLength);
        return randomString; 
    }
}