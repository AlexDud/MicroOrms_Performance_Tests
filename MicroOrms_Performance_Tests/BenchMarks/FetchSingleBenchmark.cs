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
                    var result = await session.SingleAsync<Customer>(new Guid("0EC1FD41-8ACF-4CAB-AEFF-021F29209C97"));

                    transaction.Commit();
                }
            }
        }

        [Benchmark]
        public void PP_FetchSingle()
        {
            var results = db.Single<User>(new Guid("AC26020A-3664-405E-BFF9-000BB896523A"));
        }
    }
}
