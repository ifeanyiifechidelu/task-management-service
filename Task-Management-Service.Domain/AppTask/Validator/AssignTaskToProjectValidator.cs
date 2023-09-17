using FluentValidation;
using FluentValidation.Results;

namespace Task_Management_Service.Domain;
public class AssignTaskToProjectValidator : AbstractValidator<AssignTaskToProjectDto>
{

    public AssignTaskToProjectValidator()
    {

        RuleFor(task => task.ProjectReference)
            .NotEmpty().WithMessage("Project Reference must not be empty.")
            .Must(IsGuid).WithMessage("Project Reference must be a valid GUID.");

    }

    private bool IsGuid(string guid)
    {
        return Guid.TryParse(guid, out _);
    }

    public override ValidationResult Validate(ValidationContext<AssignTaskToProjectDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(AssignTaskToProjectDto),
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}
