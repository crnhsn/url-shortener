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
                    // validate incoming POST request params via model validation
                    var validationContext = new ValidationContext(shortenRequest);
                    var validationResults = new List<ValidationResult>();

                    bool isValid = Validator.TryValidateObject(shortenRequest,
                                                               validationContext,
                                                               validationResults,
                                                               validateAllProperties:true);

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
                    // todo: catch custom exceptions here and return different error to client if needed

                    return Results.Problem(ResponseErrorMessages.InternalServerError,
                                           statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("ShortenUrl");
        
    }
    
}