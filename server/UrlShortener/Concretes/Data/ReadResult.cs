namespace UrlShortener.Concretes.Data;

public class ReadResult<T>
{
    public bool Success { get; private set; }
    public T Value { get; private set; }

    // private constructor to enforce usage of factory methods
    private ReadResult(bool success, T value)
    {
        Success = success;
        Value = value;
    }

    // factory method for successful read
    public static ReadResult<T> SuccessResult(T value)
    {
        return new ReadResult<T>(true, value);
    }

    // factory method for failed read
    public static ReadResult<T> FailureResult()
    {
        return new ReadResult<T>(false, default);
    }
}
