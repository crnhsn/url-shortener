using UrlShortener.Endpoints;
using UrlShortener.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

// register the custom services needed for the application
builder.Services.ConfigureCustomServices();

var app = builder.Build();

// map endpoints
app.MapUrlShortenEndpoint();
app.MapUrlExpansionEndpoint();


app.Run();