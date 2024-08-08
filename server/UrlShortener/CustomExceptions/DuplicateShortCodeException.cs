namespace UrlShortener.CustomExceptions;

public class DuplicateShortCodeException : System.Exception
{
    public DuplicateShortCodeException() : base("The short code is not unique.")
    {
        
    }

   
    public DuplicateShortCodeException(string message) : base(message)
    {
        
    }

    
    public DuplicateShortCodeException(string message, Exception innerException) : base(message, innerException)
    {
        
    }

    
    protected DuplicateShortCodeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
        
    }
    
}