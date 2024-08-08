using UrlShortener.Concretes.Data;
using UrlShortener.Interfaces;
using UrlShortener.Concretes.Encoders;
using UrlShortener.Concretes.Hashing;
using UrlShortener.Concretes.Randomness;
using UrlShortener.Concretes.Shorteners;
using UrlShortener.Concretes.ShorteningServices;

var builder = WebApplication.CreateBuilder(args);

const string BASE_URL = UrlShortener.Constants.BaseUrl.Stem; // todo: replace this with env var
const int SHORT_URL_LENGTH = UrlShortener.Constants.Lengths.SHORT_URL_LENGTH;

const string REDIS_CONNECTION_STRING = UrlShortener.Constants.DatabaseConnectionStrings.REDIS; // todo: replace with env var

const int MAX_SHORTEN_RETRIES = 3;

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

builder.Services.AddSingleton<IUrlShortenerService>(provider =>
    new UniqueUrlShorteningService(provider.GetRequiredService<IShorteningProvider<string, string>>(),
                                   provider.GetRequiredService<IDataRepository<string, string>>(),
                                   MAX_SHORTEN_RETRIES,
                                   BASE_URL));

var app = builder.Build();

// todo: update this to post request
// todo: add functionality for custom short link 
app.MapGet("/shorten/{longUrl}", async (string longUrl, IUrlShortenerService urlShortener) =>
{
    // todo: add validation for incoming strings, etc.


    try
    {
        string shortenedUrl = await urlShortener.CreateShortUrl(longUrl);
        return Results.Ok(shortenedUrl);
    }
    catch (Exception ex)
    {
        return Results.StatusCode(404); // todo: update error handling / status code
    }
})
.WithName("ShortenUrl");

app.MapGet("/expand/{shortUrl}", async (string shortUrl, IDataRepository<string, string> database) =>
    {
        // todo: add validation for incoming strings, etc.

        ReadResult<string> readResult = await database.TryReadAsync(shortUrl);
        if (readResult.Success)
        {
            return Results.Ok(readResult.Value);
        }
        else
        {
            return Results.StatusCode(404); // todo: update http code here / error handling here
        }


    }).WithName("ExpandUrl");

app.Run();