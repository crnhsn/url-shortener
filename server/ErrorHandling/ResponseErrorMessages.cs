namespace UrlShortener.ErrorHandling;

public static class ResponseErrorMessages
{
    public const string ShortCodeMissing = "SHORT_CODE_MISSING";
    public const string ShortCodeIsNotAlphanumeric = "SHORT_CODE_FORMAT";
    public const string ShortCodeTooLong = "SHORT_CODE_LENGTH";
    public const string ShortCodeNotFound = "SHORT_CODE_NOT_FOUND";
    
    public const string UrlNotProvided = "URL_REQUIRED";
    public const string UrlInvalid = "URL_INVALID";
    public const string UrlTooLong = "URL_LENGTH";

    public const string CustomAliasNotAlphanumeric = "CUSTOM_ALIAS_FORMAT";
    public const string CustomAliasTooLong = "CUSTOM_ALIAS_LENGTH";
    public const string CustomAliasUnavailable = "CUSTOM_ALIAS_UNAVAILABLE";

    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
    
}