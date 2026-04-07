using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Template.Api.Models.Foundation.Organization;
using Template.Api.Models.Foundation.Organization.Exceptions;
namespace Template.Api.Services.Foundations.Organizations;

// Delegate class for handling exceptions
// in the organizationService.cs file.
public partial class OrganizationService
{
    private delegate ValueTask<Organization?> ReturningOrgnizationFunction();
    private delegate IQueryable<Organization> ReturningOrganizationsFunction();

    // Exception wrapper for single organization operations.
    // Logging should be handled here as well. 
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
        catch (AlreadyExistsOrganizationException alreadyExistsOrganizationException)
        {
            throw CreateAndLogValidationException(alreadyExistsOrganizationException);
        }
        catch (NullOrganizationException nullOrganizationException)
        {
            throw CreateAndLogValidationException(nullOrganizationException);
        }
        catch (InvalidOrganizationException invalidOrganizationException)
        {
            throw CreateAndLogValidationException(invalidOrganizationException);
        }
        catch (OrganizationException organizationException)
        {
            throw CreateAndLogValidationException(organizationException);
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
        catch (SqlException sqlException)
        {
            var failedOrganizationStorageException = new FailedOrganizationStorageException(sqlException);
            throw CreateAndLogCriticalDependencyException(failedOrganizationStorageException);
        }
        catch (OrganizationException organizationException)
        {
            throw CreateAndLogServiceException(organizationException);
        }
        catch (Exception exception)
        {
            this.loggingBroker.LogError(exception);
            throw new Exception("Failed to retrieve organizations.", exception);
        }
    }

    // Helper for 400 Errors
    private OrganizationValidationException CreateAndLogValidationException(Exception exception)
    {
        var organizationValidationException = new OrganizationValidationException(exception);
        this.loggingBroker.LogError(organizationValidationException);
        return organizationValidationException;
    }

    // Helper for 500 Errors
    private OrganizationServiceException CreateAndLogServiceException(Exception exception)
    {
        var organizationServiceException = new OrganizationServiceException(exception);
        this.loggingBroker.LogError(organizationServiceException);
        return organizationServiceException;
    }

    private OrganizationDependencyException CreateAndLogCriticalDependencyException(Exception exception)
    {
        var organizationDependencyException = new OrganizationDependencyException(exception);
        this.loggingBroker.LogCritical(organizationDependencyException);
        return organizationDependencyException;
    }
}