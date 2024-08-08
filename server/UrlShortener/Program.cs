using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using UrlShortener.Concretes.Encoders;
using UrlShortener.Concretes.Hashing;
using UrlShortener.Concretes.Randomness;
using UrlShortener.Concretes.Shorteners;

using UrlShortener.Interfaces;

var builder = WebApplication.CreateBuilder(args);

const int SHORT_URL_LENGTH = UrlShortener.Constants.Lengths.SHORT_URL_LENGTH;

// Register dependencies
builder.Services.AddSingleton<IHashProvider<string, byte[]>, MD5HashProvider>();

builder.Services.AddSingleton<IEncoder<byte[], string>, UrlSafeBase64Encoder>();

builder.Services.AddSingleton<IRandomnessProvider<string>>(provider => new UrlSafeRandomStringProvider(SHORT_URL_LENGTH));

builder.Services.AddSingleton<IShorteningProvider<string, string>>(provider =>
    new RandomizedHashBasedShortener(SHORT_URL_LENGTH,
        provider.GetRequiredService<IHashProvider<string, byte[]>>(),
        provider.GetRequiredService<IEncoder<byte[], string>>(),
        provider.GetRequiredService<IRandomnessProvider<string>>()));

var app = builder.Build();

app.MapGet("/shorten/{url}", (string url, IShorteningProvider<string, string> shorteningService) =>
{
    string shortenedUrl = UrlShortener.Constants.BaseUrl.Stem + shorteningService.Shorten(url);
    return Results.Ok(shortenedUrl);
})
.WithName("ShortenUrl");

app.Run();