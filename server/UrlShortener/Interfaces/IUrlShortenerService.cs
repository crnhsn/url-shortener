namespace UrlShortener.Interfaces;

public interface IUrlShortenerService
{
    Task<string> CreateShortUrl(string longUrl);
    Task<string> ResolveShortUrl(string shortUrl);
}