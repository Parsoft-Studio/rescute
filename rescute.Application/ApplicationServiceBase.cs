using rescute.Domain.Repositories;
using rescute.Infrastructure;

namespace rescute.Application;

public abstract class ApplicationServiceBase
{
    private readonly Func<IUnitOfWork> unitOfWorkFactory;
    private readonly IApplicationConfiguration applicationConfiguration;

    private ApplicationServiceBase()
    {
        // parameterized constructor must be called
    }

    protected ApplicationServiceBase(Func<IUnitOfWork> unitOfWorkFactory,
        IApplicationConfiguration applicationConfiguration)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.applicationConfiguration = applicationConfiguration;
    }

    protected IUnitOfWork GetUnitOfWork()
    {
        return unitOfWorkFactory.Invoke();
    }

    protected IApplicationConfiguration GetConfiguration()
    {
        return applicationConfiguration;
    }
}