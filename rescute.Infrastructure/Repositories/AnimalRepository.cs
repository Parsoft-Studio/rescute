using rescute.Domain.Aggregates;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

public class AnimalRepository : Repository<Animal>, IAnimalRepository
{
    public AnimalRepository(rescuteContext c) : base(c)
    {
    }
}