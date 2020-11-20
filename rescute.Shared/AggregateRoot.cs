using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Shared
{
    public abstract class AggregateRoot<T> : Entity<T> where T : Entity<T>
    {

    }
}
