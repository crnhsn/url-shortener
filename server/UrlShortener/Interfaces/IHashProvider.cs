namespace UrlShortener.Interfaces;

public interface IHashProvider<T, U>
{
    
    public U ComputeHash(T toHash);
    
}