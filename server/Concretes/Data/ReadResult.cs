namespace UrlShortener.Concretes.Data;

public class ReadOperation<T>
{
    public bool FoundValue { get; private set; }
    public T Value { get; private set; }

    // private constructor to enforce usage of factory methods
    private ReadOperation(bool success, T value)
    {
        FoundValue = success;
        Value = value;
    }

    // factory method for read that found data
    public static ReadOperation<T> FoundResult(T value)
    {
        return new ReadOperation<T>(true, value);
    }

    // factory method for failed read
    public static ReadOperation<T> NoResult()
    {
        return new ReadOperation<T>(false, default(T));
    }
}
