using FluentValidation;
using FluentValidation.Results;

namespace Task_Management_Service.Domain;
public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectValidator()
    {
        RuleFor(project => project.Name)
            .NotEmpty().WithMessage("Name must not be empty.")
            .Matches("^[a-zA-Z]+$").WithMessage("Name can only contain letters.");

        RuleFor(project => project.Description)
            .NotEmpty().WithMessage("Description must not be empty.")
            .Length(10, 2000)
            .WithMessage("Description should be between 10 and 2000 characters.");
    }

    public override ValidationResult Validate(ValidationContext<CreateProjectDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(CreateProjectDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}
