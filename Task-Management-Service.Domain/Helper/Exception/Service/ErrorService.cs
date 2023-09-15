using FluentValidation.Results;
using Serilog;

namespace Task_Management_Service.Domain
{
    public class ErrorService : IAppExceptionService
    {
        public AppException GetValidationExceptionResult(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                Log.Information("Compiling validation errors...");

                var errorMessages = validationResult.Errors.Select(failure => failure.ErrorMessage).ToList();

                var errorMessage = string.Join("; ", errorMessages);
                
                return new BadRequestException(errorMessage);
            }

            return null;
        }
    }
}
