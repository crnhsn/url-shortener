namespace UrlShortener.InputValidation;

using System;

public static class UrlValidator
{
    private static string[] HTTP_PREFIXES = {"https://", "http://"};
    
    public static bool IsValidUrl(string input, out string? validatedUrl)
    {
        // check if the input is already a well-formed URL
        if (isWellFormedUrl(input))
        {
            validatedUrl = input; 
            return true;
        }

        // if url isn't valid, try adding http(s) prefixes to see if that makes it valid 
        foreach (string httpPrefix in HTTP_PREFIXES)
        {
            string urlWithScheme = httpPrefix + input;
            if (isWellFormedUrl(urlWithScheme))
            {
                validatedUrl = urlWithScheme; 
                return true;
            }
        }

        validatedUrl = null; 
        return false;
    }
   
    private static bool isWellFormedUrl(string url)
    {
        // urls are absolute URIs - they should contain all of the info to point to a resource
        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            Uri uri = new Uri(url);
            
            if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            {
                return true;
            }
        }

        return false;
    }
}
