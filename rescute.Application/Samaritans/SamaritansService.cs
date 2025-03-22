using rescute.Domain.Aggregates;
using rescute.Domain.Repositories;

namespace rescute.Application.Samaritans;

public class SamaritansService(Func<IUnitOfWork> unitOfWorkFactory, IApplicationConfiguration applicationConfiguration)
    : ApplicationServiceBase(unitOfWorkFactory, applicationConfiguration), ISamaritansService
{
    public async Task<Samaritan> GetOneSamaritan()
    {
        await using var unitOfWork = GetUnitOfWork();
        return (await unitOfWork.Samaritans.GetAllAsync())[0];
    }
}