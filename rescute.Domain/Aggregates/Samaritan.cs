using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Aggregates
{
    public class Samaritan : AggregateRoot<Samaritan>
    {
        private  Samaritan()
        { }

        public Samaritan(Name firstName, Name lastName, PhoneNumber mobile)
        {
            if (!mobile.IsMobile) throw new Domain.Exceptions.MobilePhoneExpected();
            FirstName = firstName;
            LastName = lastName;
            Mobile = mobile;
        }

        public Name FirstName { get; private set; }
        public Name LastName { get; private set; }

        public PhoneNumber Mobile { get; private set; }

    }
}
