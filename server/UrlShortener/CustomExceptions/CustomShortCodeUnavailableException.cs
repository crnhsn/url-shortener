namespace UrlShortener.CustomExceptions;

public class CustomShortCodeUnavailableException : System.Exception
{
    public CustomShortCodeUnavailableException() : base("The custom short code provided is not unique.")
    {
        
    }

   
    public CustomShortCodeUnavailableException(string message) : base(message)
    {
        
    }

    
    public CustomShortCodeUnavailableException(string message, Exception innerException) : base(message, innerException)
    {
        
    }

    
    protected CustomShortCodeUnavailableException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
        
    }
    
}