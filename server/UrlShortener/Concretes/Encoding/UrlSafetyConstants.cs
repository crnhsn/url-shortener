namespace UrlShortener.Concretes.Encoding;

public static class UrlSafetyConstants
{
    public static readonly List<char> URL_UNSAFE_CHARACTERS = new List<char>() {'+', '=', '/'};
    
}