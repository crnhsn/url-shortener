FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UrlShortener.csproj", "./"]
RUN dotnet restore "UrlShortener.csproj"
COPY . .
RUN dotnet publish "UrlShortener.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.dll"]