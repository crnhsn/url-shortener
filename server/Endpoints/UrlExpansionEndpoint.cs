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
                // bind the incoming short code to a model for validation
                
                var expansionRequest = new ExpansionRequest(shortCode);

                bool isValid = ModelValidator.Validate(expansionRequest,
                    out var validationResults);


                if (!isValid)
                {
                    List<string?> errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
                    return Results.BadRequest(errorMessages);
                }

                bool shortCodeUnused = await urlShortener.IsShortCodeAvailable(shortCode);

                // if a short code isn't in use, then we can't redirect using it 
                if (shortCodeUnused)
                {
                    return Results.NotFound(ResponseErrorMessages.ShortCodeNotFound);
                }

                string longUrl = await urlShortener.ResolveShortUrl(shortCode);

                return Results.Redirect(longUrl);
            }
            catch (Exception ex)
            {
                // todo: log exception

                return Results.Problem(ResponseErrorMessages.InternalServerError,
                                       statusCode: StatusCodes.Status500InternalServerError);
            }
    
        }).WithName("ExpandUrl");
    }
    
}