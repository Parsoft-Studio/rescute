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

        public Samaritan(Name firstName, Name lastName, PhoneNumber mobile,DateTime registrationDate)
        {
            if (!mobile.IsMobile) throw new Domain.Exceptions.MobilePhoneExpected();
            FirstName = firstName;
            LastName = lastName;
            Mobile = mobile;
            RegistrationDate = registrationDate;
        }

        public DateTime RegistrationDate { get; private set; }
        public Name FirstName { get; private set; }
        public Name LastName { get; private set; }

        public PhoneNumber Mobile { get; private set; }

        public static Samaritan RandomTestSamaritan()
        {
            return new Samaritan(new Name("Test First"), new Name("Test Last"), new PhoneNumber(true, "0912121212"), DateTime.Now);
        }
    }
}
