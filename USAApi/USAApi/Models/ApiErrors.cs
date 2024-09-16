using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace USAApi.Models
{
    public class ApiErrors
    {
        public ApiErrors()
        {
            
        }
        public ApiErrors(string message)
        {
            Message = message;
        }
        public ApiErrors(ModelStateDictionary modelState)
        {
            Message = "Invalid parameters.";
            Details = modelState
                .FirstOrDefault(x => x.Value.Errors.Any()).Value.Errors
                .FirstOrDefault().ErrorMessage;
        }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
