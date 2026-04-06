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
        // null check.
        ValidateOrganizationIsNotNull(organization);

        // Iterates through each rule and applies
        // it to the associated parameter.
        Validate(
            // Each Tuple contains a rule and a parameter
            (Rule: IsInvalid(organization.Id), parameter: nameof(Organization.Id)),
            (Rule: IsInvalid(organization.Name), parameter: nameof(Organization.Name)),
            (Rule: IsInvalid(organization.Status), parameter: nameof(Organization.Status))
        );
    }

    /// <summary>
    /// Checks if the object exists.
    /// </summary>
    /// <param name="organization"></param>
    /// <exception cref="NullOrganizationException"></exception>
    private static void ValidateOrganizationIsNotNull(Organization organization)
    {
        if (organization is null)
        {
            throw new NullOrganizationException();
        }
    }

    private async ValueTask ValidateOrganizationDoesNotAlreadyExistAsync(Organization organization)
    {
        Organization? existingOrganization =
            await this.RetrieveOrganizationByIdAsync(organization.Id);

        if (existingOrganization is not null)
        {
            throw new AlreadyExistsOrganizationException(organization.Id);
        }
    }

    /// <summary>
    /// Rule: Id must not be null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };

    /// <summary>
    /// Name must not be null.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static dynamic IsInvalid(string text) => new
    {
        Condition = string.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };

    /// <summary>
    /// This is the validate engine that takes
    /// in rules and the values they should be
    /// applied to.
    /// </summary>
    /// <param name="validations"></param>
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