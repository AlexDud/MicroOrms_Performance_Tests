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
    public class UpdateBenchMarks
    {
        private readonly ISessionFactory sessionFactory;
        private readonly IDatabase db;

        public UpdateBenchMarks()
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
                    var customer = await session.SingleAsync<Customer>(new Guid("0EC1FD41-8ACF-4CAB-AEFF-021F29209C97"));
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
            var user = db.Single<User>(new Guid("AC26020A-3664-405E-BFF9-000BB896523A"));
            user.Name = "Randomich12";
            user.Phone = "1234567890";
            user.Age = 25;
            user.UserStatus = UserStatus.Updated;

            db.Update(user);
        }
    }
}