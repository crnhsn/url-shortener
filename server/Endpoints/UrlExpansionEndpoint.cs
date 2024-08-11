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
                // so send a 404 back along with server-side HTML
                if (shortCodeUnused)
                {
                    return getShortCodeNotFoundResponse();
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
    
    private static IResult getShortCodeNotFoundResponse()
    {
        string notFoundHtml = @"<html>
                            <body>
                    <h1>404 Not Found - This short code is not in use.</h1>
                    <a href='/'>Back to home</a>
                            </body>
                           </html>";

        return Results.Text(content: notFoundHtml,
                    contentType: "text/html",
                    statusCode: (int?)System.Net.HttpStatusCode.NotFound);

    }

}