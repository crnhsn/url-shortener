namespace UrlShortener.Interfaces;

public interface IUrlShortener
{
    string Shorten(string longUrl);
    string Expand(string shortUrl);
}