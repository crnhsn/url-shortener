using System.ComponentModel.DataAnnotations;
using UrlShortener.Constants;
using UrlShortener.ErrorHandling;

namespace UrlShortener.Endpoints.RequestModels;

public class ExpansionRequest
{
    [Required(ErrorMessage = ResponseErrorMessages.ShortCodeMissing)]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = ResponseErrorMessages.ShortCodeIsNotAlphanumeric)]
    [StringLength(Lengths.MAX_LONG_URL_LENGTH, ErrorMessage = ResponseErrorMessages.ShortCodeTooLong)]
    public string ShortCode {get; set;}

    public ExpansionRequest(string shortCode)
    {
        ShortCode = shortCode;
    }
}