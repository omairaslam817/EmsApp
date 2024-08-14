using Ems.Domain.Errors;
using Ems.Domain.Primitives;
using Ems.Domain.Shared;

namespace Ems.Domain.ValueObjects;

public sealed class TransactionNumber : ValueObject
{
    private TransactionNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<TransactionNumber> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return Result.Failure<TransactionNumber>(DomainErrors.TransactionNumber.Empty);
        }

        if (number.Length <= 2)
        {
            return Result.Failure<TransactionNumber>(DomainErrors.TransactionNumber.TooLong);
        }

        return new TransactionNumber(number);
    }

    // Override ToString()
    public override string ToString()
    {
        return Value;
    }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}