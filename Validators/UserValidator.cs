using FluentValidation;
using Template.Api.Models.Foundation.User;

namespace Template.Api.Validators;

public class UserValidator : AbstractValidator<User>
{
    private static readonly string[] AllowedRoles = { "Admin", "Manager", "User", "ReadOnly" };

    public UserValidator()
    {
        // -------------------------
        // UserId
        // -------------------------
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required.");

        // -------------------------
        // OrganizationId
        // -------------------------
        RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage("Organization ID is required");

        // -------------------------
        // FirstName
        // -------------------------
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 character")
            .Matches(@"^[a-zA-Z\s\-']+$")
            .WithMessage("First name can only contain letters, spaces, hyphens, and apostrophes.");

        // -------------------------
        // LastName
        // -------------------------
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters.")
            .MaximumLength(100)
            .WithMessage("Last name must not exceed 100 characters")
            .Matches(@"^[a-zA-Z\s\-']+$")
            .WithMessage("First name can only contain letters, spaces, hyphens, and apostrophes.");

        // -------------------------
        // Email
        // -------------------------
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email field is required.")
            .EmailAddress()
            .WithMessage("Email must be in xxxx@x.com/net/gov format.");

        // -------------------------
        // Phone
        // -------------------------
        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be a valid format (e.g. +12125551234).");

        // -------------------------
        // Role
        // -------------------------
        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required.")
            .Must(role => AllowedRoles.Contains(role))
            .WithMessage($"Role must be one of: {string.Join(", ", AllowedRoles)}.");

        //TODO: Validate Registration Date, IsActive, PasswordHash
        // Update program.cs
    }
}