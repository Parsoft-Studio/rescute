using rescute.Domain.Repositories;
using rescute.Infrastructure;
using rescute.Infrastructure.Repositories;

namespace rescute.Tests.InfrastructureTests;

public abstract class RepositoryTestBase
{
    /// <summary>
    ///     Creates an <see cref="IUnitOfWork" /> to be used in tests.
    /// </summary>
    protected static IUnitOfWork GetUnitOfWork()
    {
        var context = new rescuteContext(TestUtility.GetTestDatabaseOptions());
        return new UnitOfWork(context);
    }
}