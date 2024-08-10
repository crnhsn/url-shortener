namespace UrlShortener.Constants;

public static class UrlSafety
{
    public static readonly List<char> URL_UNSAFE_CHARACTERS = new List<char>() {'+', '=', '/'};
    
    public static readonly string URL_SAFE_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

}