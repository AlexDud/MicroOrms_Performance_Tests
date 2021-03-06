﻿using System;
using System.Linq;
using FluentAssertions;
using PetaPoco;
using Ploeh.AutoFixture;
using Xunit;

namespace MicroOrms_Performance_Tests.PetaPoco
{
    public class PetaPocoTests
    {
        private readonly IDatabase db;
        private readonly Fixture fixture;

        public PetaPocoTests()
        {
            fixture = new Fixture();
            db = PetaPocoSetup.GetDb();
        }

        [Fact]
        public void truncate_users_table_tests()
        {
            int result = db.Execute("TRUNCATE TABLE Users");

            result.Should().Be(-1);
        }

        [Fact]
        public void insert_into_users_table_tests()
        {
            var user = fixture.Create<User>();

            db.Insert(user);
        }

        [Fact]
        public void update_users_table_tests()
        {
            var user = db.Single<User>(new Guid("1a96f313-610a-442c-bdba-cfc3b3a23770"));
            user.Name = "Randomich";
            user.Phone = "1234567890";
            user.Age = 25;
            user.UserStatus = UserStatus.Updated;

            db.Update(user);
        }

        [Fact]
        public void get_all_data_from_users_table_tests()
        {
            var users = db.Query<User>("SELECT TOP (100) * FROM [Users]");

            users.Count().Should().BeGreaterThan(0);
        }
    }
}
