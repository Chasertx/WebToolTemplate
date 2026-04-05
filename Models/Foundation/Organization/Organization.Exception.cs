using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Template.Api.Models.Foundation.Organization.Exceptions;

public class OrganizationException : Exception
{
    public OrganizationException(string message, Exception innerException)
        : base(message, innerException) { }

    public OrganizationException(string message) : base(message) { }
}

public class OrganizationValidationException : OrganizationException
{
    public OrganizationValidationException(Exception innerException)
        : base(message: "Organization validation error occurred, fix the error and try again.",
            innerException)
    { }
}

public class NullOrganizationException : OrganizationException
{
    public NullOrganizationException()
        : base(message: "Organization is null.") { }
}

public class InvalidOrganizationException : OrganizationException
{
    public InvalidOrganizationException()
        : base(message: "Organization is invalid.") { }

    //adds an error to the collection
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
