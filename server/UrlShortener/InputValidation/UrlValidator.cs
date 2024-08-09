using System;

namespace UrlShortener.InputValidation
{
    public static class UrlValidator
    {
        private static readonly string[] HTTP_PREFIXES = { "https://", "http://" };
    
        // todo: make url validation more robust
        public static bool IsValidUrl(string input, out string? validatedUrl)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                validatedUrl = null;
                return false;
            }

            // check if input is already well-formed url
            if (isWellFormedUrl(input))
            {
                validatedUrl = input;
                return true;
            }

            // if url isn't initially valid, try adding http(s) prefix to see if that makes it valid
            foreach (string httpPrefix in HTTP_PREFIXES)
            {
                string prefixedUrl = httpPrefix + input;
                if (isWellFormedUrl(prefixedUrl))
                {
                    validatedUrl = prefixedUrl;
                    return true;
                }
            }

            validatedUrl = null;
            return false;
        }

        private static bool isWellFormedUrl(string url)
        {
            // try to create a uri from the input and validate http / https,
            // host existence, and that there are at least two parts
            // separated by a . in the host
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
            {
                bool isHttpOrHttps = uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
                bool hostExists = !string.IsNullOrWhiteSpace(uri.Host);

                return isHttpOrHttps && hostExists && isValidHostFormat(uri.Host);
            }

            return false;
        }

        private static bool isValidHostFormat(string host, char separator = '.')
        {
            // todo: this could be made better by also confirming that the host is a valid domain
            // but that would likely require a network call (e.g., DNS lookup) to actually be sure, which might
            // negatively impact performance
            // could also try a regex, todo
            string[] parts = host.Split(separator);
            return parts.Length >= 2;
        }
    }
}
