using FluentValidation;
using FluentValidation.Results;

namespace Task_Management_Service.Domain;
public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    private static readonly string[] ValidPriorityTypes = { "Low", "Medium", "High" };
    private static readonly string[] ValidStatusTypes = { "Pending", "In-Progress", "Completed" };

    public CreateTaskValidator()
    {
        RuleFor(task => task.Title)
            .NotEmpty().WithMessage("Title must not be empty.")
            .Length(5, 200)
            .WithMessage("Title should be between 5 and 200 characters.");

        RuleFor(task => task.Description)
            .NotEmpty().WithMessage("Description must not be empty.")
            .Length(10, 2000)
            .WithMessage("Description should be between 10 and 2000 characters.");

        RuleFor(task => task.DueDate)
            .NotEmpty().WithMessage("Due Date must not be empty.");

        RuleFor(task => task.Priority)
            .NotEmpty().WithMessage("Priority must not be empty.")
            .Must(type => ValidPriorityTypes.Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Priority Type must be either 'Low', 'Medium' or 'High'.");

        RuleFor(task => task.Status)
            .NotEmpty().WithMessage("Status must not be empty.")
            .Must(type => ValidStatusTypes.Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Status Type must be either 'Pending', 'In-Progress', 'Completed'.");

        RuleFor(task => task.UserReference)
            .NotEmpty().WithMessage("User Reference must not be empty.")
            .Must(IsGuid).WithMessage("User Reference must be a valid GUID.");

        RuleFor(task => task.ProjectReference)
            .NotEmpty().WithMessage("Project Reference must not be empty.")
            .Must(IsGuid).WithMessage("Project Reference must be a valid GUID.");

    }

    private bool IsGuid(string guid)
    {
        return Guid.TryParse(guid, out _);
    }

    public override ValidationResult Validate(ValidationContext<CreateTaskDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(CreateTaskDto),
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}
