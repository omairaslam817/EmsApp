﻿using Ems.Domain.Errors;
using Ems.Domain.Primitives;
using Ems.Domain.Shared;

namespace Ems.Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    private FirstName()
    {
    }

    public string Value { get; private set; }

    public static Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<FirstName>(DomainErrors.FirstName.Empty);
        }

        if (firstName.Length > MaxLength)
        {
            return Result.Failure<FirstName>(DomainErrors.FirstName.TooLong);
        }

        return new FirstName(firstName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
