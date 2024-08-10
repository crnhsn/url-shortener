using System.ComponentModel.DataAnnotations;
using UrlShortener.Interfaces;

namespace UrlShortener.Endpoints;

public static class UrlExpansionEndpoint
{
    private static class ErrorMessages
    {
        public const string ShortCodeMissing = "SHORT_CODE_MISSING";
        public const string ShortCodeIsNotAlphanumeric = "SHORT_CODE_FORMAT";
        public const string ShortCodeTooLong = "SHORT_CODE_LENGTH";

        public const string ShortCodeNotFound = "SHORT_CODE_NOT_FOUND";

        public const string InternalServerError = "INTERNAL_SERVER_ERROR";


    }
    private class ExpansionRequest
    {
        [Required(ErrorMessage = ErrorMessages.ShortCodeMissing)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = ErrorMessages.ShortCodeIsNotAlphanumeric)]
        [StringLength(Constants.Lengths.MAX_LONG_URL_LENGTH, ErrorMessage = ErrorMessages.ShortCodeTooLong)]
        public string ShortCode {get; set;}

        public ExpansionRequest(string shortCode)
        {
            ShortCode = shortCode;
        }
    }
    
    public static void MapUrlExpansionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{shortCode}", async (string shortCode, IUrlShortenerService urlShortener) =>
        {
            try
            {
                // bind the incoming short code to a model so validation of the short code
                // can just use existing model validation flow instead of needing custom flow
                
                var expansionRequest = new ExpansionRequest(shortCode);

                var validationContext = new ValidationContext(expansionRequest);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(expansionRequest,
                                                           validationContext,
                                                           validationResults,
                                                           validateAllProperties:true);

                if (!isValid)
                {
                    List<string?> errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
                    return Results.BadRequest(errorMessages);
                }

                string longUrl = await urlShortener.ResolveShortUrl(shortCode);

                return Results.Redirect(longUrl);
            }
            catch (Exception ex)
            {
                // todo: log exception?
                // todo - catch custom exceptions here and send 404 if Resolve above doesn't find anything

                return Results.Problem(ErrorMessages.InternalServerError,
                                       statusCode: StatusCodes.Status500InternalServerError);
            }
    
        }).WithName("ExpandUrl");
    }
    
}