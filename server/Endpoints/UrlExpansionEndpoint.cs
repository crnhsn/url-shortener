using System.ComponentModel.DataAnnotations;
using UrlShortener.Endpoints.RequestModels;
using UrlShortener.Interfaces;
using UrlShortener.ErrorHandling;

namespace UrlShortener.Endpoints;

public static class UrlExpansionEndpoint
{
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

                return Results.Problem(ResponseErrorMessages.InternalServerError,
                                       statusCode: StatusCodes.Status500InternalServerError);
            }
    
        }).WithName("ExpandUrl");
    }
    
}