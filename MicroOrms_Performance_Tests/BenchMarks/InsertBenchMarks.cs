using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using MicroLite;
using MicroOrms_Performance_Tests.MicroLite;
using MicroOrms_Performance_Tests.PetaPoco;
using PetaPoco;
using Ploeh.AutoFixture;

namespace MicroOrms_Performance_Tests.BenchMarks
{
    [MinColumn, MaxColumn]
    [Config(typeof(BenchMarksConfig))]
    public class InsertBenchMarks
    {
        private readonly Fixture fixture;
        private readonly ISessionFactory sessionFactory;
        private readonly IDatabase db;

        public InsertBenchMarks()
        {
            fixture = new Fixture();

            sessionFactory = MicroLiteSetup.GetSessionFactory();
            db = PetaPocoSetup.GetDb();
        }

        [Benchmark]
        public async Task ML_InsertData()
        {
            var customer = fixture.Create<Customer>();

            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    await session.InsertAsync(customer);

                    transaction.Commit();
                }
            }
        }

        [Benchmark]
        public void PP_InsertData()
        {
            var user = fixture.Create<User>();

            db.Insert(user);
        }
    }
}