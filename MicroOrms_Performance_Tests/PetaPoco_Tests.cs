using FluentAssertions;
using PetaPoco;
using Ploeh.AutoFixture;
using Xunit;

namespace MicroOrms_Performance_Tests
{
    public class PetaPocoTests
    {
        private readonly Database db;
        private readonly Fixture fixture;

        public PetaPocoTests()
        {
            fixture = new Fixture();
            db = new Database("TestConnection");
            PocoData.FlushCaches();
        }

        [Fact]
        public void insert_into_users_table_performanse_tests()
        {
            var performanceResult = PerformanceChecker.check_average_execution_time(() =>
            {
                var user = fixture.Create<User>();

                db.Insert("Users", user);
            });

            performanceResult.Should().BeGreaterThan(0);
        }

        [Fact]
        public void truncate_users_table_performanse_tests()
        {
            int result = db.Execute("TRUNCATE TABLE Users");

            result.Should().Be(-1);
        }
    }
}
