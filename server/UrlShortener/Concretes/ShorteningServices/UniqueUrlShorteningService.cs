using UrlShortener.Concretes.Data;
using UrlShortener.Interfaces;

namespace UrlShortener.Concretes.ShorteningServices;

public class UniqueUrlShorteningService : IUrlShortenerService
{
    private readonly IShorteningProvider<string, string> _shortener;
    private readonly IDataRepository<string, string> _dataStore;
    
    private readonly int MAX_SHORTEN_RETRIES;
    private readonly string BASE_URL; 
        
    public UniqueUrlShorteningService(IShorteningProvider<string, string> shortener,
                                      IDataRepository<string, string> dataStore,
                                      int maxShortenRetries = 3,
                                      string baseUrl = UrlShortener.Constants.BaseUrl.Stem)
    {
        _shortener = shortener;
        _dataStore = dataStore;
        
        MAX_SHORTEN_RETRIES = maxShortenRetries;
        BASE_URL = baseUrl; 
    }
    
    public async Task<string> CreateShortUrl(string longUrl)
    {
        // todo: add validation for incoming strings, etc.
        // validation might be better as middleware at the top level
        
        string shortCode = _shortener.Shorten(longUrl);
        bool shortCodeAlreadyExistsInDataStore = await _dataStore.KeyExistsAsync(shortCode);
        
        if (shortCodeAlreadyExistsInDataStore)
        {
            int retryCount = 0;
        
        while (shortCodeAlreadyExistsInDataStore && retryCount < MAX_SHORTEN_RETRIES)
        {
            // todo: improve this retry logic somehow - maybe a retry controller of some kind 
            // probably need to account for various flows here - e.g., user passed in a custom url vs not
            // if user passed in custom, and it already exists, vs if user didn't pass in random one
            shortCode = _shortener.Shorten(longUrl);
            shortCodeAlreadyExistsInDataStore = await _dataStore.KeyExistsAsync(shortCode);
            retryCount += 1; 
        
        }
        
        // if we've exhausted the number of retries
        // without managing to generate a unique URL short code not already in the data store
        if (shortCodeAlreadyExistsInDataStore)
        {
            throw new Exception($"Failed to generate a unique short code for {longUrl}. " +
                $"Attempted unique short code generation {MAX_SHORTEN_RETRIES + 1} times."); 
        }
        }
        
        bool writeSucceeded = await _dataStore.TryWriteAsync(shortCode, longUrl);
    
        // todo: what to do if write didn't succeed? error? retry? error for now, but maybe retry
        // based on the reason it didn't succeed (e.g., have a WriteResult that stores exception info)
        
        if (writeSucceeded)
        {
            string shortenedUrl = BASE_URL + shortCode;
            return shortenedUrl; 
        }
        else
        {
            throw new Exception("Generated shortened URL, but failed to write shortened URL to data store.");
        }
    }

    public async Task<string> ResolveShortUrl(string shortUrl)
    {
        ReadResult<string> readResult = await _dataStore.TryReadAsync(shortUrl);
        if (readResult.Success)
        {
            return readResult.Value;
        }
        else
        {
            throw new Exception($"Unable to resolve {shortUrl}"); // todo: update ReadResult to store exceptions, and add to this
        }
    }
    
}