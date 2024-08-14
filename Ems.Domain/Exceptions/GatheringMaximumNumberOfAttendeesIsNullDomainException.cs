namespace Ems.Domain.Exceptions;

public sealed class GatheringMaximumNumberOfAttendeesIsNullDomainException : DomainException
{
    public GatheringMaximumNumberOfAttendeesIsNullDomainException(string message) : base(message)
    {
    }
}