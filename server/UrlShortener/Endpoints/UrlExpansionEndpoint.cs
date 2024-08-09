using UrlShortener.Interfaces;

namespace UrlShortener.Endpoints;

public static class UrlExpansionEndpoint
{
    
    public static void MapUrlExpansionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/expand/{shortUrl}", async (string shortUrl, IUrlShortenerService urlShortener) =>
        {
            // todo: add validation for incoming strings, etc.
            // e.g., can't expand short URLs that don't have the expected base URL
            // probably a library to do this
    
            try
            {
                string longUrl = await urlShortener.ResolveShortUrl(shortUrl);
                return Results.Ok(longUrl);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(404); // todo: update error handling / status code, maybe via global handler for exceptions
            }
    
        }).WithName("ExpandUrl");
    }
    
}