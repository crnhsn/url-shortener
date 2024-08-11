using UrlShortener.Concretes.Data;

namespace UrlShortener.Interfaces;

public interface IDataRepository<T, U>
{
    
    public Task<ReadOperation<U>> TryReadAsync(T key);
    public Task<bool> TryWriteAsync(T key, U value);
    public Task<bool> KeyExistsAsync(T key);
    
}