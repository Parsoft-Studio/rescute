﻿using System;
using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates;

public class Samaritan : AggregateRoot<Samaritan>
{
    private Samaritan()
    {
    }

    public Samaritan(Name firstName, Name lastName, PhoneNumber mobile, DateTime registrationDate)
    {
        if (!mobile.IsMobile) throw new MobilePhoneExpectedException();
        FirstName = firstName;
        LastName = lastName;
        Mobile = mobile;
        RegistrationDate = registrationDate;
    }

    public DateTime RegistrationDate { get; private set; }
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }

    public PhoneNumber Mobile { get; private set; }
}