using Ems.Domain.Shared;

namespace Ems.Domain.Errors;

public static class DomainErrors
{
    public static class Member
    {
        public static readonly Error EmailAlreadyInUse = new(
            "Member.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Member.NotFound",
            $"The member with the identifier {id} was not found.");

        public static readonly Error InvalidCredentials = new(
            "Member.InvalidCredentials",
            "The provided credentials are invalid");
    }
    public static class TransactionDate
    {
        public static readonly Error Empty = new(
            "TransactionDate.Empty",
            "TransactionDate is empty.");

        public static readonly Error InvalidFormat = new(
            "TransactionDate.InvalidFormat",
            "Transaction Date format is invalid.");
    }

    public static class TransactionNumber
    {
        public static readonly Error Empty = new(
            "TransactionNumber.Empty",
            "Transaction Number is empty.");

        public static readonly Error TooLong = new(
            "TransactionNumber.TooLong",
            "Transaction Number is too long.");
    }
    public static class TransactionAmount
    {
        public static readonly Error Empty = new(
            "TransactionAmount.Empty",
            "Transaction Amount is empty.");

        public static readonly Error TooLong = new(
            "TransactionAmount.TooLong",
            "Transaction Amount is too long.");
    }
    public static class Balance
    {
        public static readonly Error Empty = new(
            "Balance.Empty",
            "Balance Amount is empty.");

        public static readonly Error TooLong = new(
            "Balance.TooLong",
            "Balance Amount is too long.");
    }
    public static class TransactionDetails
    {
        public static readonly Error Empty = new(
            "TransactionNumber.Empty",
            "Transaction Number is empty.");

    }
    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");

        public static readonly Error TooLong = new(
            "Email.TooLong",
            "Email is too long");

        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "Email format is invalid");
    }
    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "FirstName name is too long");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }

    public static class Gathering
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Gathering.NotFound",
            $"The gathering with the identifier {id} was not found.");

        public static readonly Error InvitingCreator = new(
            "Gathering.InvitingCreator",
            "Can't send invitation to the gathering creator");

        public static readonly Error AlreadyPassed = new(
            "Gathering.AlreadyPassed",
            "Can't send invitation for gathering in the past");

        public static readonly Error Expired = new(
            "Gathering.Expired",
            "Can't accept invitation for expired gathering");
    }

}