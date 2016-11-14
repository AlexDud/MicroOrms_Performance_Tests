using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using MicroLite;
using MicroLite.Configuration;
using MicroLite.Listeners;
using MicroLite.Mapping;
using Ploeh.AutoFixture;
using Xunit;

namespace MicroOrms_Performance_Tests
{
    public class GuidListener : IInsertListener
    {
        public void AfterInsert(object instance, object executeScalarResult)
        {
            return; // nothing to do
        }

        public void BeforeInsert(object instance)
        {
            var objectInfo = ObjectInfo.For(instance.GetType());

            if (objectInfo.TableInfo.IdentifierStrategy == IdentifierStrategy.Assigned
                && objectInfo.TableInfo.IdentifierColumn.PropertyInfo.PropertyType == typeof(Guid))
            {
                var identifier = Guid.NewGuid();

                objectInfo.SetIdentifierValue(instance, identifier);
            }
        }
    }

    public class MicroLiteTests
    {
        private readonly Fixture fixture;
        private readonly ISessionFactory sessionFactory;

        public MicroLiteTests()
        {
            fixture = new Fixture();

            Listener.InsertListener.Add(new GuidListener());

            Configure.Extensions()
                .WithConventionBasedMapping(
                    new ConventionMappingSettings
                    {
                        ResolveIdentifierStrategy = type => IdentifierStrategy.Assigned
                    });

            sessionFactory = Configure
                 .Fluently()
                 .ForMsSql2012Connection("TestConnection")
                 .CreateSessionFactory();
        }

        [Fact]
        public void insert_into_customers_table_performanse_tests()
        {
            var performanceResult = PerformanceChecker.check_average_execution_time(async () =>
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
            });

            performanceResult.Should().BeGreaterThan(0);
        }

        [Fact]
        public void get_all_data_from_customers_table_performanse_tests()
        {
            IList<Customer> customers;

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    //var query = new SqlQuery("SELECT [CustomerId], [Name], [Surname], [Phone], [Email], [Age], [DateOfBirth], [CustomerStatusId] FROM [Customers]");

                    var query = new SqlQuery("SELECT * FROM [Customers]");

                    customers = session.Fetch<Customer>(query);

                    transaction.Commit();
                }
            }

            var a = customers;
        }

        [Fact]
        public async Task update_customers_table_performanse_tests()
        {
            bool updated;

            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var customer = await session.SingleAsync<Customer>(new Guid("0AEFDBFD-222B-4798-89C2-0092D9CF778E"));
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
        public async Task delete_from_customers_table_performanse_tests()
        {
            bool deleted;

            using (var session = sessionFactory.OpenAsyncSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    deleted = await session.Advanced.DeleteAsync(typeof(Customer), new Guid("17975F2B-804B-486F-97BD-E568DF231E27"));

                    transaction.Commit();
                }
            }

            deleted.Should().BeTrue();
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
    }
}
