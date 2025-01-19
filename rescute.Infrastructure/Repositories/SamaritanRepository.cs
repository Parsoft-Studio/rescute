using rescute.Domain.Aggregates;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

internal class SamaritanRepository : Repository<Samaritan>, ISamaritanRepository
{
    public SamaritanRepository(rescuteContext context) : base(context)
    {
    }
}