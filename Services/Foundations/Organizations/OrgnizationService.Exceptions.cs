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
    private async ValueTask<Organization?> TryCatch(ReturningOrgnizationFunction returningOrganizationFunction)
    {
        try
        {
            Organization? org = await returningOrganizationFunction();

            //logging success.
            this.loggingBroker.LogInformation($"Successfully persisted organization: {org?.Name}");
            return org;
        }
        catch (DbUpdateException dbUpdateException)
        {
            this.loggingBroker.LogError(dbUpdateException);
            throw new Exception("Database error occurred.", dbUpdateException);
        }
        catch (Exception exception)
        {
            this.loggingBroker.LogError(exception);
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
            this.loggingBroker.LogError(exception);
            throw new Exception("Failed to retrieve organizations.", exception);
        }
    }
}