using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using USAApi.Models;

namespace USAApi.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _env;
        public JsonExceptionFilter(IHostEnvironment env)
        {
            _env = env; //injecting hosting environment interface
        }
        public void OnException(ExceptionContext context)
        {
            var error = new ApiErrors();
            if(_env.IsDevelopment())
            {
                error.Message = context.Exception.Message;
                error.Details = context.Exception.StackTrace;
            }
            else
            {
                error.Message = "A Server error occured.";
                error.Details = context.Exception.Message;
            }
            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
        }
    }
}
