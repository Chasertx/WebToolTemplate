using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Template.Api.Models.Foundation.Organization;
using Template.Api.Services.Foundations.Organizations;
using Template.Api.Models.Foundation.Organization.Exceptions;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;

namespace Template.Api.Services.Foundations.Organizations;

/// <summary>
/// This our "Rule-Book" in essense, it defines what
/// consitutes a valid organization.
/// </summary>
public partial class OrganizationService
{
    // High-level validation entry point
    private void ValidateOrganizationOnAdd(Organization organization)
    {
        ValidateOrganizationIsNotNull(organization);

        Validate(
            // Each Tuple contains a rule and a parameter
            (Rule: IsInvalid(organization.Id), parameter: nameof(Organization.Id)),
            (Rule: IsInvalid(organization.Name), parameter: nameof(Organization.Name)),
            (Rule: IsInvalid(organization.Status), parameter: nameof(Organization.Status))
        );

        this.loggingBroker.LogInformation($"Successfully validated organization: {organization?.Name}");
    }

    private static void ValidateOrganizationIsNotNull(Organization organization)
    {
        if (organization is null)
        {
            throw new NullOrganizationException();
        }
    }

    // Rule: Guid must not be empty
    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };

    // Rule: String must not be null, empty or whitespace
    private static dynamic IsInvalid(string text) => new
    {
        Condition = string.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };

    // The Standard "Validate" engine to collect all errors.
    private static void Validate(params (dynamic Rule, string parameter)[] validations)
    {
        var invalidOrganizationException = new InvalidOrganizationException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidOrganizationException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }

            invalidOrganizationException.ThrowIfContainsErrors();
        }
    }
}