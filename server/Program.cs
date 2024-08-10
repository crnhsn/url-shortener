using UrlShortener.Endpoints;
using UrlShortener.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

// register the custom services needed for the application
builder.Services.ConfigureCustomServices();

// register CORS allow list
builder.Services.ConfigureCors();

var app = builder.Build();

app.UseCors("AllowList"); // todo: put this policy name in env var or constant 

// map endpoints
app.MapUrlShortenEndpoint();
app.MapUrlExpansionEndpoint();


app.Run();