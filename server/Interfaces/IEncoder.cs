namespace UrlShortener.Interfaces;

public interface IEncoder<T, U>
{
    
    public U Encode(T toEncode); 
    
}