namespace UrlShortener.Interfaces;

public interface IDataRepository<T, U>
{
    
    public bool TryRead(T key, out U value);
    public bool TryWrite(T key, U value);
    
}