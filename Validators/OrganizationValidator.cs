using FluentValidation;
using Template.Api.Models.Foundation.Organization;

namespace Template.Api.Validators;

public class OrganizationValidator : AbstractValidator<Organization>
{
    private static readonly string[] ValidStatuses = ["Active", "Inactive", "Suspended"];

    public OrganizationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Organization Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}.");
    }

}
