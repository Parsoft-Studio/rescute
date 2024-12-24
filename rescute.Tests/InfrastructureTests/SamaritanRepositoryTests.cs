using System.Threading.Tasks;
using FluentAssertions;
using rescute.Infrastructure;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

// [Collection("Database collection")]
public class SamaritanRepositoryTests
{
    [Fact]
    public async Task SamaritanRepositoryAddsAndGetsSamaritan()
    {
        using (var context = new rescuteContext(TestUtility.GetTestDatabaseOptions()))
        {
            using (var unitOfWork = new UnitOfWork(context))
            {
                var samaritan = TestUtility.RandomTestSamaritan();

                unitOfWork.Samaritans.Add(samaritan);
                await unitOfWork.Complete();
                var same = await unitOfWork.Samaritans.GetAsync(samaritan.Id);


                same.Should().NotBe(null);
                same.Should().Be(samaritan);
            }
        }
    }
}