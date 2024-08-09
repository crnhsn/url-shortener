using UrlShortener.InputValidation;
using UrlShortener.Interfaces;

namespace UrlShortener.Endpoints;

public static class UrlExpansionEndpoint
{
    
    public static void MapUrlExpansionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{shortUrl}", async (string shortUrl, IUrlShortenerService urlShortener) =>
        {
            // todo: add validation for incoming strings, etc.
            // e.g., can't expand short URLs that don't have the expected base URL
            // probably a library to do this
    
            try
            {
                string longUrl = await urlShortener.ResolveShortUrl(shortUrl);
                bool urlIsValid = UrlValidator.IsValidUrl(longUrl, out string validatedUrl);
                if (!urlIsValid)
                {
                    return Results.StatusCode(404); // not found or invalid
                }
                return Results.Redirect(validatedUrl);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
    
        }).WithName("ExpandUrl");
    }
    
}