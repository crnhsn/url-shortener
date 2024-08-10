using System.ComponentModel.DataAnnotations;
using UrlShortener.InputValidation;
using UrlShortener.Interfaces;

namespace UrlShortener.Endpoints;

public static class UrlShortenEndpoint
{
    private class ShortenRequest
    {
        [Required(ErrorMessage = "URL_REQUIRED")]
        [Url(ErrorMessage = "INVALID_URL")]

        public string LongUrl {get; set;}

        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "CUSTOM_ALIAS_NON_ALPHANUMERIC")]
        [StringLength(Constants.Lengths.SHORT_URL_LENGTH, ErrorMessage = "CUSTOM_ALIAS_LENGTH")]
        public string? CustomAlias {get; set;}
    }

    public static void MapUrlShortenEndpoint(this IEndpointRouteBuilder app)
    {
        
        app.MapPost("/shorten", async (ShortenRequest shortenRequest, IUrlShortenerService urlShortener) =>
            {
                // todo: add validation for incoming strings, etc.

                
            
                try
                {
                    string longUrl = shortenRequest.LongUrl;
                    string? customAlias = shortenRequest.CustomAlias;


                    bool userHasRequestedCustomAlias = !String.IsNullOrWhiteSpace(customAlias);
            
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