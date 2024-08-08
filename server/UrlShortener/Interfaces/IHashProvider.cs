namespace UrlShortener.Interfaces;

public interface IHashProvider<T, U>
{
    
    public T ComputeHash(U toHash);
    
}