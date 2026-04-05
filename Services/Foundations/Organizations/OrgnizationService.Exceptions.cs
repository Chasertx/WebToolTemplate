using Microsoft.EntityFrameworkCore;
using Template.Api.Models.Foundation.Organization;
namespace Template.Api.Services.Foundations.Organizations;

// Delegate class for handling exceptions
// in the organizationService.cs file.
public partial class OrganizationService
{
    private delegate ValueTask<Organization?> ReturningOrgnizationFunction();
    private delegate IQueryable<Organization> ReturningOrganizationsFunction();

    // Exception wrapper for single organization operations.
    private ValueTask<Organization?> TryCatch(ReturningOrgnizationFunction returningOrganizationFunction)
    {
        try
        {
            return returningOrganizationFunction();
        }
        catch (DbUpdateException dbUpdateException)
        {
            throw new Exception("Database error occurred.", dbUpdateException);
        }
        catch (Exception exception)
        {
            throw new Exception("Service error occurred.", exception);
        }
    }

    // Exception wrapper for multiple organization operations.
    private IQueryable<Organization> TryCatch(ReturningOrganizationsFunction returningOrganizationsFunction)
    {
        try
        {
            return returningOrganizationsFunction();
        }
        catch (Exception exception)
        {
            throw new Exception("Failed to retrieve organizations.", exception);
        }
    }
}