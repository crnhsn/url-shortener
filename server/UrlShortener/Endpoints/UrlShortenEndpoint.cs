using System.ComponentModel.DataAnnotations;
using UrlShortener.InputValidation;
using UrlShortener.Interfaces;

namespace UrlShortener.Endpoints;

public static class UrlShortenEndpoint
{
    private static class ErrorMessages
    {
        public const string UrlNotProvided = "URL_REQUIRED";
        public const string UrlInvalid = "URL_INVALID";

        public const string CustomAliasNotAlphanumeric = "CUSTOM_ALIAS_FORMAT";
        public const string CustomAliasTooLong = "CUSTOM_ALIAS_LENGTH";
        public const string CustomAliasUnavailable = "CUSTOM_ALIAS_UNAVAILABLE";

        public const string InternalServerError = "INTERNAL_SERVER_ERROR";

    }

    private class ShortenRequest
    {
        [Required(ErrorMessage = ErrorMessages.UrlNotProvided)]
        [Url(ErrorMessage = ErrorMessages.UrlInvalid)]

        public string LongUrl {get; set;}

        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = ErrorMessages.CustomAliasNotAlphanumeric)]
        [StringLength(Constants.Lengths.SHORT_URL_LENGTH, ErrorMessage = ErrorMessages.CustomAliasTooLong)]
        public string? CustomAlias {get; set;}
    }

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
                            return Results.Conflict(ErrorMessages.CustomAliasUnavailable);
                        }
                    }

                    string shortenedUrl = await urlShortener.CreateShortUrl(longUrl, customAlias);
                    
                    return Results.Ok(shortenedUrl);
                }
                catch (Exception ex)
                {
                    // todo: log exception?

                    return Results.Problem(ErrorMessages.InternalServerError);
                }
            })
            .WithName("ShortenUrl");
            
        
    }
    
}