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
                            return Results.StatusCode(404); // todo: update error handling / status code here, maybe throw custom exception for duplicate custom code that global exception handler catches and returns to client
                        }
                    }
            
                    string shortenedUrl = await urlShortener.CreateShortUrl(longUrl, customAlias);
                    return Results.Ok(shortenedUrl);
                }
                catch (Exception ex)
                {
                    return Results.StatusCode(404); // todo: update error handling / status code
                }
            })
            .WithName("ShortenUrl");
            
        
    }
    
}