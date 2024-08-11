using System.ComponentModel.DataAnnotations;
using UrlShortener.Endpoints.RequestModels;
using UrlShortener.Interfaces;
using UrlShortener.ErrorHandling;

namespace UrlShortener.Endpoints;

public static class UrlShortenEndpoint
{
    public static void MapUrlShortenEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/shorten", async (ShortenRequest shortenRequest, IUrlShortenerService urlShortener) =>
            {
                try
                {
                    // validate incoming POST request body
                    // by binding to data model and validating
                    bool isValid = ModelValidator.Validate(shortenRequest, out var validationResults);

                    if (!isValid)
                    {
                        List<string?> errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
                        return Results.BadRequest(errorMessages);
                    }

                    string longUrl = shortenRequest.LongUrl;
                    string? customAlias = shortenRequest.CustomAlias;

                    bool userHasRequestedCustomAlias = !String.IsNullOrWhiteSpace(customAlias);
            
                    if (userHasRequestedCustomAlias)
                    {
                        bool isCustomAliasAvailable = await urlShortener.IsShortCodeAvailable(customAlias);
            
                        if (!isCustomAliasAvailable)
                        {
                            return Results.Conflict(ResponseErrorMessages.CustomAliasUnavailable);
                        }
                    }

                    string shortenedUrl = await urlShortener.CreateShortUrl(longUrl, customAlias);
                    
                    return Results.Ok(shortenedUrl);
                }
                catch (Exception ex)
                {
                    // todo: log exception?

                    return Results.Problem(ResponseErrorMessages.InternalServerError,
                                           statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("ShortenUrl");
        
    }
    
}