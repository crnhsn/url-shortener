namespace UrlShortener.ErrorHandling.CustomExceptions;

public class ShortCodeException : System.Exception
{
    public ShortCodeException() : base("There was an issue with the short code.")
    {
        
    }

   
    public ShortCodeException(string message) : base(message)
    {
        
    }

    
    public ShortCodeException(string message, Exception innerException) : base(message, innerException)
    {
        
    }

    
    protected ShortCodeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
        
    }
    
}