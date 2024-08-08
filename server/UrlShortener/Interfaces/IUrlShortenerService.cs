namespace UrlShortener.Interfaces;

public interface IUrlShortenerService
{
    string Shorten(string longUrl);
    string Expand(string shortUrl);
}