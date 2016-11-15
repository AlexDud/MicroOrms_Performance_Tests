using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using MicroLite;
using MicroOrms_Performance_Tests.MicroLite;
using MicroOrms_Performance_Tests.PetaPoco;
using PetaPoco;

namespace MicroOrms_Performance_Tests.BenchMarks
{
    [MinColumn, MaxColumn]
    [Config(typeof(BenchMarksConfig))]
    public class SelectBenchMarks
    {
        private readonly ISessionFactory sessionFactory;
        private readonly IDatabase db;

        public SelectBenchMarks()
        {
            sessionFactory = MicroLiteSetup.GetSessionFactory();
            db = PetaPocoSetup.GetDb();
        }

        [Benchmark]
        public async Task ML_SelectAllData()
        {
            using (var session = sessionFactory.OpenAsyncReadOnlySession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var customers = await session.FetchAsync<Customer>(new SqlQuery("SELECT * FROM [Customers]"));

                    transaction.Commit();
                }
            }
        }

        [Benchmark]
        public void PP_SelectAllData()
        {
            var users = db.Query<User>("SELECT * FROM [Users]");
        }
    }
}
