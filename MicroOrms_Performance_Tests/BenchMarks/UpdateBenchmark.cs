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
    public class UpdateBenchmark
    {
        private readonly ISessionFactory sessionFactory;
        private readonly IDatabase db;

        public UpdateBenchmark()
        {
            sessionFactory = MicroLiteSetup.GetSessionFactory();
            db = PetaPocoSetup.GetDb();
        }

        [Benchmark]
        public async Task ML_UpdateData()
        {
            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var customer = await session.SingleAsync<Customer>(new Guid("0ec1fd41-8acf-4cab-aeff-021f29209c97"));
                    customer.Name = "Randomich12";
                    customer.Phone = "1234567890";
                    customer.Age = 25;
                    customer.Status = CustomerStatus.Closed;

                    var updated = await session.UpdateAsync(customer);

                    transaction.Commit();
                }
            }
        }

        [Benchmark]
        public void PP_UpdateData()
        {
            var user = db.Single<User>(new Guid("1a96f313-610a-442c-bdba-cfc3b3a23770"));
            user.Name = "Randomich12";
            user.Phone = "1234567890";
            user.Age = 25;
            user.UserStatus = UserStatus.Updated;

            db.Update(user);
        }
    }
}