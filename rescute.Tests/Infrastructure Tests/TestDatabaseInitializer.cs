using rescute.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace rescute.Tests.InfrastructureTests
{
    /// <summary>
    /// This class drops and recreates the test database to be used by Infrastructure tests.
    /// </summary>
    public class TestDatabaseInitializer : IDisposable
    {
        public static string TestsConnectionString => "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=rescuteTest;Integrated Security=SSPI;";
        public TestDatabaseInitializer()
        {

            using (var context = new rescuteContext(TestsConnectionString))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public void Dispose() { }


    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<TestDatabaseInitializer>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
