﻿using Ems.Domain.Primitives;
using Ems.Domain.Enums;

namespace Ems.Domain.Entities;

public sealed class Invitation : Entity
{
    internal Invitation(Guid id, Member member, Gathering gathering)
        : base(id)
    {
        MemberId = member.Id;
        GatheringId = gathering.Id;
        Status = InvitationStatus.Pending;
        CreatedOnUtc = DateTime.UtcNow;
    }

    //private Invitation()
    //{
    //}

    public Guid GatheringId { get; private set; }

    public Guid MemberId { get; private set; }

    public InvitationStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    internal void Expire()
    {
        Status = InvitationStatus.Expired;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    internal Attendee Accept()
    {
        Status = InvitationStatus.Accepted;
        ModifiedOnUtc = DateTime.UtcNow;

        var attendee = new Attendee(this);

        return attendee;
    }
}