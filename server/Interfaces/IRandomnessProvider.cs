namespace UrlShortener.Interfaces;

public interface IRandomnessProvider<T>
{
    public T GenerateRandomValue();
}