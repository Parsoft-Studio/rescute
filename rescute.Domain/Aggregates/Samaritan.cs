using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Aggregates
{
    public class Samaritan : AggregateRoot<Samaritan>
    {
        public  Samaritan()
        { }

        public Samaritan(MyName firstName, MyName lastName, PhoneNumber mobile)
        {
            FirstName = firstName;
            LastName = lastName;
            Mobile = mobile;
        }

        public MyName FirstName { get; private set; }
        public MyName LastName { get; private set; }

        public PhoneNumber Mobile { get; private set; }

    }
}
