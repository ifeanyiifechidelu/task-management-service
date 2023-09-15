using FluentValidation;
using FluentValidation.Results;

namespace Task_Management_Service.Domain;
public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("FirstName must not be empty.")
            .Matches("^[a-zA-Z]+$").WithMessage("First Name can only contain letters.");

        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("LastName must not be empty.")
            .Matches("^[a-zA-Z]+$").WithMessage("Last Name can only contain letters.");

        RuleFor(user => user.Email)
        .NotEmpty().WithMessage("Email must not be empty.")
        .EmailAddress().WithMessage("Email must be in valid format.");
    }

    public override ValidationResult Validate(ValidationContext<CreateUserDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(CreateUserDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}
