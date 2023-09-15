using System.DirectoryServices.Protocols;
using FluentValidation.Results;

namespace Task_Management_Service.Domain;
public interface IAppExceptionService
   {
      AppException GetValidationExceptionResult(ValidationResult validationResult);
      
   }