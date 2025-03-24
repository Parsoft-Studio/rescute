using rescute.Domain.Repositories;
using rescute.Infrastructure;
using rescute.Infrastructure.Repositories;

namespace rescute.Tests.InfrastructureTests;

public abstract class RepositoryTestBase
{
    /// <summary>
    ///     Creates an <see cref="IUnitOfWork" /> to be used in tests.
    /// </summary>
    /// <param name="useDefaultDatabase">
    ///     When true, uses the default shared database (for tests that need multiple contexts to access the same data).
    ///     When false (default), creates a unique database for test isolation.
    /// </param>
    protected static IUnitOfWork GetUnitOfWork(bool useDefaultDatabase = false)
    {
        var context = new rescuteContext(TestUtility.GetTestDatabaseOptions(useDefaultDatabase));
        return new UnitOfWork(context);
    }
}