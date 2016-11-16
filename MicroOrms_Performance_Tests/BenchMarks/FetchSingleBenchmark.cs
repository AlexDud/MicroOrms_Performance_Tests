using System;
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
    public class FetchSingleBenchmark
    {
        private readonly ISessionFactory sessionFactory;
        private readonly IDatabase db;

        public FetchSingleBenchmark()
        {
            sessionFactory = MicroLiteSetup.GetSessionFactory();
            db = PetaPocoSetup.GetDb();
        }

        [Benchmark]
        public async Task ML_FetchSingle()
        {
            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var result = await session.SingleAsync<Customer>(new Guid("0ec1fd41-8acf-4cab-aeff-021f29209c97"));

                    transaction.Commit();
                }
            }
        }

        [Benchmark]
        public void PP_FetchSingle()
        {
            var results = db.Single<User>(new Guid("1a96f313-610a-442c-bdba-cfc3b3a23770"));
        }
    }
}
