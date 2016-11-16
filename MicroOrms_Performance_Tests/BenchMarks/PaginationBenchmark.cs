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
    public class PaginationBenchmark
    {
        private readonly ISessionFactory sessionFactory;
        private readonly IDatabase db;

        public PaginationBenchmark()
        {
            sessionFactory = MicroLiteSetup.GetSessionFactory();
            db = PetaPocoSetup.GetDb();
        }

        [Benchmark]
        public async Task ML_Pagination()
        {
            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var query = new SqlQuery("SELECT * FROM [Customers]");

                    var result = await session.PagedAsync<Customer>(query, PagingOptions.ForPage(1, 100));

                    transaction.Commit();
                }
            }
        }

        [Benchmark]
        public void PP_Pagination()
        {
            var results = db.Page<User>(1, 100, new Sql("SELECT * FROM [Users]"));
        }
    }
}
