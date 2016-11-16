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
    public class TakeSkipBenchmarks
    {
        private readonly ISessionFactory sessionFactory;
        private readonly IDatabase db;

        public TakeSkipBenchmarks()
        {
            sessionFactory = MicroLiteSetup.GetSessionFactory();
            db = PetaPocoSetup.GetDb();
        }

        [Benchmark]
        public async Task ML_TakeSkip()
        {
            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var query = new SqlQuery("SELECT * FROM [Customers]");

                    var result = await session.PagedAsync<Customer>(query, PagingOptions.SkipTake(50, 100));

                    transaction.Commit();
                }
            }
        }

        [Benchmark]
        public void PP_TakeSkip()
        {
            var results = db.SkipTake<User>(50, 100, new Sql("SELECT * FROM [Users]"));
        }
    }
}
