using rescute.Domain.Aggregates;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

internal class AnimalRepository : Repository<Animal>, IAnimalRepository
{
    public AnimalRepository(rescuteContext context) : base(context)
    {
    }
}