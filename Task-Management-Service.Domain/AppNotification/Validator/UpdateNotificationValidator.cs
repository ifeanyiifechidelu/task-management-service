using FluentValidation;
using FluentValidation.Results;

namespace Task_Management_Service.Domain;
public class UpdateNotificationValidator : AbstractValidator<UpdateNotificationDto>
{
    private static readonly string[] ValidNotificationTypes = { "Due Date Reminder", "Status Updated"};
    public UpdateNotificationValidator()
    {
        RuleFor(notification => notification.Type)
            .NotEmpty().WithMessage("Notification Type must not be empty.")
            .Must(type => ValidNotificationTypes.Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Notification Type must be either 'Due Date Reminder' or 'Status Updated'.");

        RuleFor(notification => notification.Message)
            .NotEmpty().WithMessage("Message must not be empty.")
            .Length(10, 2000)
            .WithMessage("Message should be between 10 and 2000 characters.");

        RuleFor(notification => notification.UserReference)
        .NotEmpty().WithMessage("User Reference must not be empty.")
            .Must(IsGuid).WithMessage("User Reference must be a valid GUID.");
    }

    private bool IsGuid(string guid)
    {
        return Guid.TryParse(guid, out _);
    }

    public override ValidationResult Validate(ValidationContext<UpdateNotificationDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(UpdateNotificationDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}
