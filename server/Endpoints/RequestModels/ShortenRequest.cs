using System.ComponentModel.DataAnnotations;
using UrlShortener.Constants;
using UrlShortener.ErrorHandling;

namespace UrlShortener.Endpoints.RequestModels;

public class ShortenRequest
{
    [Required(ErrorMessage = ResponseErrorMessages.UrlNotProvided)]
    [Url(ErrorMessage = ResponseErrorMessages.UrlInvalid)]
    [StringLength(Lengths.MAX_LONG_URL_LENGTH, ErrorMessage = ResponseErrorMessages.UrlTooLong)]
    public string LongUrl {get; set;}
    
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = ResponseErrorMessages.CustomAliasNotAlphanumeric)]
    [StringLength(Lengths.SHORT_URL_LENGTH, ErrorMessage = ResponseErrorMessages.CustomAliasTooLong)]
    public string? CustomAlias {get; set;}
}