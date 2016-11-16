using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MicroLite;
using Ploeh.AutoFixture;
using Xunit;

namespace MicroOrms_Performance_Tests.MicroLite
{
    public class MicroLiteTests
    {
        private readonly Fixture fixture;
        private readonly ISessionFactory sessionFactory;

        public MicroLiteTests()
        {
            fixture = new Fixture();
            sessionFactory = MicroLiteSetup.GetSessionFactory();
        }

        [Fact]
        public async Task truncate_customers_table_performanse_tests()
        {
            int result;

            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var query = new SqlQuery("TRUNCATE TABLE Customers");

                    result = await session.Advanced.ExecuteAsync(query);

                    transaction.Commit();
                }
            }

            result.Should().Be(-1);
        }

        [Fact]
        public async Task insert_into_customers_table_tests()
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

        [Fact]
        public async Task get_all_data_from_customers_table_tests()
        {
            IList<Customer> customers;

            using (var session = sessionFactory.OpenAsyncReadOnlySession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    customers = await session.FetchAsync<Customer>(new SqlQuery("SELECT TOP (100) * FROM [Customers]"));

                    transaction.Commit();
                }
            }

            customers.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task update_customers_table_tests()
        {
            bool updated;

            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var customer = await session.SingleAsync<Customer>(new Guid("0ec1fd41-8acf-4cab-aeff-021f29209c97"));
                    customer.Name = "Maxim";
                    customer.Phone = "1234567890";
                    customer.Age = 25;
                    customer.Status = CustomerStatus.Closed;

                    updated = await session.UpdateAsync(customer);

                    transaction.Commit();
                }
            }

            updated.Should().BeTrue();
        }

        [Fact]
        public async Task delete_from_customers_table_tests()
        {
            bool deleted;

            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    deleted = await session.Advanced.DeleteAsync(typeof(Customer), new Guid("0ec1fd41-8acf-4cab-aeff-021f29209c97"));

                    transaction.Commit();
                }
            }

            deleted.Should().BeTrue();
        }
    }
}
