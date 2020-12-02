﻿using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MedicinePlanner.Core.Exceptions
{
    public class ApiError
    {
        public string Message { get; set; }
        public bool IsError { get; set; }
        public string StackTrace { get; set; }

        public ApiError(string message)
        {
            this.Message = message;
            IsError = true;
        }

        public ApiError(ModelStateDictionary modelState)
        {
            this.IsError = true;
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                Message = "Please correct the errors in input data and try again.";
            }
        }
    }
}
