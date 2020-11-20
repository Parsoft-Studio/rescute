using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Aggregates
{
    public class Animal : AggregateRoot<Animal>
    {
        public AnimalType Type { get; private set; }
        public Animal(AnimalType type)
        {
            Type = type;
        }
        private Animal() { }
    }
}
