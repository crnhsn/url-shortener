using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Endpoints.RequestModels;

public static class ModelValidator
{
    public static bool Validate<T> (T modelInstance, out List<ValidationResult> validationResults)
    {
        var validationContext = new ValidationContext(modelInstance);
        
        var results = new List<ValidationResult>();
        
        bool isValid = Validator.TryValidateObject(modelInstance,
                                                   validationContext,
                                                   results,
                                                   validateAllProperties:true);
        
        validationResults = results; 
        
        return isValid; 
    }
    
}