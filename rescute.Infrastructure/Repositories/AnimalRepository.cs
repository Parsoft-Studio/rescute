using System;
using System.Collections.Generic;
using System.Text;
using rescute.Domain.Repositories;
using rescute.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace rescute.Infrastructure.Repositories
{
    public class AnimalRepository : Repository<Animal>, IAnimalRepository
    {
        public AnimalRepository(rescuteContext c) : base(c)
        {
        }
    }
}
