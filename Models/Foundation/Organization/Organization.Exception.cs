using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Template.Api.Services.Foundations.Organizations;

namespace Template.Api.Models.Foundation.Organization.Exceptions;

/// <summary>
/// Base exception for all Ogranization-related
/// issues to ensure domain-specific catching.
/// </summary>
public class OrganizationException : Exception
{
    public OrganizationException(string message, Exception innerException)
        : base(message, innerException) { }

    // Constructor for a simple message-only domain exception.
    public OrganizationException(string message) : base(message) { }
}

/// <summary>
/// Categorical exception that wraps specific
/// validation failures for the controller to
/// handle as 400 bad request.
/// </summary>
public class OrganizationValidationException : OrganizationException
{
    public OrganizationValidationException(Exception innerException)
        : base(message: "Organization validation error occurred, fix the error and try again.",
            innerException)
    { }
}

/// <summary>
/// Specific exception thrown when the input
/// Organization object is missing or null.
/// </summary>
public class NullOrganizationException : OrganizationException
{
    public NullOrganizationException()
        : base(message: "Organization is null.") { }
}

/// <summary>
/// Specific exception for duplicate organizationID
/// checks.
/// </summary>
public class AlreadyExistsOrganizationException : OrganizationException
{
    public AlreadyExistsOrganizationException(Guid userId)
        : base(message: $"Organization with Id '{userId}' already exists.") { }
}

/// <summary>
/// Collector exception used to aggregate
/// multiple business rule violations into
/// a single response.
/// </summary>
public class InvalidOrganizationException : OrganizationException
{
    // Sets the header message for a collection of field level errors.
    public InvalidOrganizationException()
        : base(message: "Organization is invalid.") { }

    // Adds or updates an error message list 
    // associated with a specific property
    // name key.
    public void UpsertDataList(string key, string value)
    {
        if (this.Data.Contains(key))
        {
            (this.Data[key] as List<string>)?.Add(value);
        }
        else
        {
            this.Data.Add(key, new List<string> { value });
        }

    }

    public void ThrowIfContainsErrors()
    {
        if (this.Data.Count > 0)
        {
            throw this;
        }
    }
}

/// <summary>
/// Catches 500 errors.
/// </summary>
public class OrganizationServiceException : OrganizationException
{
    public OrganizationServiceException(Exception innerException)
        : base(message: "Organization service error occurred, contact support.",
        innerException)
    { }
}

/// <summary>
/// For depenency errors.
/// </summary>
public class OrganizationDependencyException : OrganizationException
{
    public OrganizationDependencyException(Exception innerException)
        : base(message: "Organization dependency exception, contact support.",
        innerException)
    { }
}

/// <summary>
/// 
/// </summary>
public class FailedOrganizationStorageException : OrganizationException
{
    public FailedOrganizationStorageException(Exception innerException)
        : base(message: "Failed to write to database.",
        innerException)
    { }
}


