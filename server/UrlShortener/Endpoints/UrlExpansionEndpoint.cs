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
                // ensure longUrl is valid URL before issuing a redirect
                // if not valid URL (empty, null, etc.), throw exception or handle error somehow

                return Results.Redirect(longUrl);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(404); // todo: update error handling / status code, maybe via global handler for exceptions
            }
    
        }).WithName("ExpandUrl");
    }
    
}