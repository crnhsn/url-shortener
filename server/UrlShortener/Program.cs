using UrlShortener.Concretes.Data;
using UrlShortener.Interfaces;
using UrlShortener.Concretes.Encoders;
using UrlShortener.Concretes.Hashing;
using UrlShortener.Concretes.Randomness;
using UrlShortener.Concretes.Shorteners;

var builder = WebApplication.CreateBuilder(args);

const string BASE_URL = UrlShortener.Constants.BaseUrl.Stem; // todo: replace this with env var
const int SHORT_URL_LENGTH = UrlShortener.Constants.Lengths.SHORT_URL_LENGTH;

const string REDIS_CONNECTION_STRING = UrlShortener.Constants.DatabaseConnectionStrings.REDIS; // todo: replace with env var

const int MAX_SHORTEN_RETRIES = 5;

// Register dependencies
builder.Services.AddSingleton<IHashProvider<string, byte[]>, MD5HashProvider>();

builder.Services.AddSingleton<IEncoder<byte[], string>, UrlSafeBase64Encoder>();

builder.Services.AddSingleton<IRandomnessProvider<string>>(new UrlSafeRandomStringProvider(SHORT_URL_LENGTH));

builder.Services.AddSingleton<IDataRepository<string, string>>(new RedisDataRepository(REDIS_CONNECTION_STRING));

builder.Services.AddSingleton<IShorteningProvider<string, string>>(provider =>
    new RandomizedHashBasedShortener(SHORT_URL_LENGTH,
        provider.GetRequiredService<IHashProvider<string, byte[]>>(),
        provider.GetRequiredService<IEncoder<byte[], string>>(),
        provider.GetRequiredService<IRandomnessProvider<string>>()));

var app = builder.Build();

// todo: update this to post request
// todo: add functionality for custom short link 
app.MapGet("/shorten/{urlToShorten}", async (string urlToShorten,
        IShorteningProvider<string, string> shorteningService,
        IDataRepository<string, string> database) =>
{
    string shortenedUrl = BASE_URL + shorteningService.Shorten(urlToShorten);
    bool shortUrlAlreadyExists = await database.KeyExistsAsync(shortenedUrl);

    if (shortUrlAlreadyExists)
    {
        // todo: add some kind of retry logic? retry X times before returning 404 to client?
        // probably need to account for various flows here - e.g., user passed in a custom url vs not
        // if user passed in custom, and it alraedy exists, vs if user didn't pass in random one
        int retryCount = 0;

        while (shortUrlAlreadyExists && retryCount < MAX_SHORTEN_RETRIES)
        {
            shortenedUrl = BASE_URL + shorteningService.Shorten(urlToShorten);
            shortUrlAlreadyExists = await database.KeyExistsAsync(shortenedUrl);
            retryCount += 1;
        }
    }

    // if we've exhausted the number of retries without seeing a new URL, return some kind of error
    if (shortUrlAlreadyExists)
    {
        return Results.StatusCode(404); // todo: update http code here
    }

    await database.TryWriteAsync(shortenedUrl, urlToShorten);

    return Results.Ok(shortenedUrl);
})
.WithName("ShortenUrl");

app.Run();