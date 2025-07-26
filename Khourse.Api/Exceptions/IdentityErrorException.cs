using System;
using Microsoft.AspNetCore.Identity;

namespace Khourse.Api.Exceptions;

public class IdentityErrorException(IEnumerable<IdentityError> errors) : Exception("Validation failed")
{
    public IEnumerable<IdentityError> Errors { get; } = errors;
}
