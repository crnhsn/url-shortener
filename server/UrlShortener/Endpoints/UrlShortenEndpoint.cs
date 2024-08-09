using UrlShortener.InputValidation;
using UrlShortener.Interfaces;

namespace UrlShortener.Endpoints;

public static class UrlShortenEndpoint
{
    public static void MapUrlShortenEndpoint(this IEndpointRouteBuilder app)
    {
        
        app.MapPost("/shorten", async (string longUrl, string? customAlias, IUrlShortenerService urlShortener) =>
            {
                // todo: add validation for incoming strings, etc.
            
                try
                {
                    bool userHasRequestedCustomAlias = !String.IsNullOrEmpty(customAlias);
            
                    if (userHasRequestedCustomAlias)
                    {
                        bool isCustomAliasAvailable = await urlShortener.IsShortCodeAvailable(customAlias);
            
                        if (!isCustomAliasAvailable)
                        {
                            return Results.Ok("url already taken"); // todo: update error handling / status code here, maybe throw custom exception for duplicate custom code that global exception handler catches and returns to client
                        }
                    }

                    bool urlIsValid = UrlValidator.IsValidUrl(longUrl, out string validatedLongUrl);
                    
                    if (!urlIsValid)
                    {
                        return Results.Ok("invalid input url"); // todo: update error handling, status code, etc. here - throw custom?
                    }
            
                    string shortenedUrl = await urlShortener.CreateShortUrl(validatedLongUrl, customAlias);
                    return Results.Ok(shortenedUrl);
                }
                catch (Exception ex)
                {
                    return Results.Ok(ex.ToString()); // todo: update error handling / status code
                }
            })
            .WithName("ShortenUrl");
            
        
    }
    
}