using UrlShortener.Concretes.Data;
using UrlShortener.Interfaces;
using UrlShortener.Concretes.Encoders;
using UrlShortener.Concretes.Hashing;
using UrlShortener.Concretes.Randomness;
using UrlShortener.Concretes.Shorteners;
using UrlShortener.Concretes.ShorteningService;
using UrlShortener.Config;
using UrlShortener.Constants;


namespace UrlShortener.ServiceConfiguration;

public static class ServiceManager
{

    const int MAX_SHORTEN_RETRIES = 3;
    

    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        
        // Register dependencies
        services.AddSingleton<IHashProvider<string, byte[]>, MD5HashProvider>();
        
        services.AddSingleton<IEncoder<byte[], string>, UrlSafeBase64Encoder>();
        
        services.AddSingleton<IRandomnessProvider<string>>(new UrlSafeRandomStringProvider(Lengths.SHORT_URL_LENGTH));
        
        services.AddSingleton<IDataRepository<string, string>>(new RedisDataRepository(EnvironmentalVariables.REDIS_CONNECTION_STRING));
        
        services.AddSingleton<IShorteningProvider<string, string>>(provider =>
            new RandomizedHashBasedShortener(Lengths.SHORT_URL_LENGTH,
                provider.GetRequiredService<IHashProvider<string, byte[]>>(),
                provider.GetRequiredService<IEncoder<byte[], string>>(),
                provider.GetRequiredService<IRandomnessProvider<string>>()));
        
        services.AddSingleton<IUrlShortenerService>(provider =>
            new UniqueUrlShorteningService(provider.GetRequiredService<IShorteningProvider<string, string>>(),
                                           provider.GetRequiredService<IDataRepository<string, string>>(),
                                           MAX_SHORTEN_RETRIES
                                           ));
        
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowList", policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(EnvironmentalVariables.ALLOWED_CORS_ORIGIN)  // Use environment variable values
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}