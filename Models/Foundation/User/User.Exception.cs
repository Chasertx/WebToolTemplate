using System;
using System.Collections.Generic;

namespace Template.Api.Models.Foundation.User.Exceptions;

public class UserException : Exception
{
    public UserException(string message, Exception innerException)
        : base(message, innerException) { }

    public UserException(string message) : base(message) { }
}

public class UserValidationException : UserException
{
    public UserValidationException(Exception innerException)
        : base(message: "User validation error occurred, fix the error and try again.",
            innerException)
    { }
}

public class NullUserException : UserException
{
    public NullUserException()
        : base(message: "User is null.") { }
}

public class AlreadyExistsUserException : UserException
{
    public AlreadyExistsUserException(Guid userId)
        : base(message: $"User with Id '{userId}' already exists.") { }
}

public class InvalidUserException : UserException
{
    public InvalidUserException()
        : base(message: "User is invalid.") { }

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
