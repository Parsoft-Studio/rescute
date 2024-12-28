using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

public class SamaritanRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task SamaritanRepositoryAddsAndGetsSamaritan()
    {
        await using var unitOfWork = GetUnitOfWork();
        var samaritan = TestUtility.RandomTestSamaritan();

        unitOfWork.Samaritans.Add(samaritan);
        await unitOfWork.Complete();
        var stored = await unitOfWork.Samaritans.GetAsync(samaritan.Id);


        stored.Should().NotBe(null);
        stored.Should().Be(samaritan);
    }
}