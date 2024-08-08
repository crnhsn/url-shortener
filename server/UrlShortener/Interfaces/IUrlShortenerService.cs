namespace UrlShortener.Interfaces;

public interface IUrlShortenerService
{
    string CreateShortUrl(string longUrl);
    string ResolveShortUrl(string shortUrl);
}