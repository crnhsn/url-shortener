using UrlShortener.Concretes.Data;
using UrlShortener.Config;
using UrlShortener.ErrorHandling.CustomExceptions;
using UrlShortener.Interfaces;

namespace UrlShortener.Concretes.ShorteningService;

public class UniqueUrlShorteningService : IUrlShortenerService
{
    private readonly IShorteningProvider<string, string> _shortener;
    private readonly IDataRepository<string, string> _dataStore;
    
    private readonly int MAX_SHORTEN_RETRIES;
    private readonly string BASE_URL; 
        
    public UniqueUrlShorteningService(IShorteningProvider<string, string> shortener,
                                      IDataRepository<string, string> dataStore,
                                      int maxShortenRetries)
    {
        _shortener = shortener;
        _dataStore = dataStore;
        
        MAX_SHORTEN_RETRIES = maxShortenRetries;
        BASE_URL = EnvironmentalVariables.BASE_URL;
    }


    public async Task<string> CreateShortUrl(string longUrl, string? customShortCode = null)
    {
        
        string shortCode = String.Empty;

        if (String.IsNullOrWhiteSpace(customShortCode))
        {
            shortCode = await createShortCodeFromLongUrl(longUrl);
        }
        else
        {
            shortCode = customShortCode;
        }

        if (String.IsNullOrWhiteSpace(shortCode))
        {
            throw new ShortCodeException("Short code should not be empty or null at this point.");
        }

        bool writeSucceeded = await writeShortCodeToDataStore(shortCode, longUrl);

        if (!writeSucceeded)
        {
            throw new ShortCodeException("Did not successfully write short code to data store.");
        }

        string shortenedUrl = BASE_URL + shortCode;

        return shortenedUrl;
    }

    public async Task<string> ResolveShortUrl(string shortUrl)
    {
        ReadOperation<string> readResult = await _dataStore.TryReadAsync(shortUrl);
        if (readResult.FoundValue)
        {
            return readResult.Value;
        }
        else
        {
            throw new ShortCodeException($"Unable to resolve {shortUrl}");
        }
    }

    public async Task<bool> IsShortCodeAvailable(string shortCode)
    {
        bool shortCodeAlreadyExists = await _dataStore.KeyExistsAsync(shortCode);
        return !shortCodeAlreadyExists;
    }


    private async Task<string> createShortCodeFromLongUrl(string longUrl)
    {
        string shortCode = _shortener.Shorten(longUrl);
        bool shortCodeAvailable = await IsShortCodeAvailable(shortCode);

        if (!shortCodeAvailable)
        {
            shortCode = await retryShortCodeGeneration(longUrl, MAX_SHORTEN_RETRIES);
        }

        return shortCode;
    }

    private async Task<bool> writeShortCodeToDataStore(string shortCode, string longUrl)
    {
        bool writeSucceeded = await _dataStore.TryWriteAsync(shortCode, longUrl);
        return writeSucceeded;
    }

    private async Task<string> retryShortCodeGeneration(string longUrl, int maxRetries)
    {
        bool shortCodeAlreadyExists = true;
        int retryCount = 0;
        string shortCode = String.Empty;

        while (shortCodeAlreadyExists && retryCount < maxRetries)
        {
            shortCode = _shortener.Shorten(longUrl);
            shortCodeAlreadyExists = !await IsShortCodeAvailable(shortCode);
            retryCount += 1;

        }

        // if we've exhausted the number of retries
        // without managing to generate a unique URL short code not already in the data store
        if (shortCodeAlreadyExists)
        {
            throw new ShortCodeException($"Failed to generate a unique short code for {longUrl}. " +
                $"Attempted unique short code generation {MAX_SHORTEN_RETRIES + 1} times.");
        }

        return shortCode;
    }
}