namespace UrlShortener.Config;

public static class EnvironmentalVariables
{
    public static readonly string BASE_URL = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ?? "localhost:3000/";
    public static readonly string REDIS_CONNECTION_STRING = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? "localhost:6379";
    public static readonly string ALLOWED_CORS_ORIGIN = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ?? "localhost:3000/"; 
    
}