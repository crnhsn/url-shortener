namespace UrlShortener.Interfaces;

public interface IUrlShortenerService
{
    Task<string> CreateShortUrl(string longUrl, string? customShortCode);
    Task<string> ResolveShortUrl(string shortUrl);
    Task<bool> IsShortCodeAvailable(string shortCode); 

}