namespace UrlShortener.Interfaces;

public interface IShorteningProvider<T, U>
{
    public T Shorten(U toShorten);
}