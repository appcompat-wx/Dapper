using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace Dapper.Tests
{
    /// <summary>
    /// If Docker Desktop is installed, run the following command to start a container suitable for the tests.
    /// <code>
    /// docker run -d -p 5432:5432 --name Dapper.Tests.PostgreSQL -e POSTGRES_DB=dappertest -e POSTGRES_USER=dappertest -e POSTGRES_PASSWORD=dapperpass postgres
    /// </code>
    /// </summary>
    public class PostgresProvider : DatabaseProvider
    {
        public override DbProviderFactory Factory => Npgsql.NpgsqlFactory.Instance;
        public override string GetConnectionString() =>
            GetConnectionString("PostgesConnectionString", "Server=localhost;Port=5432;User Id=dappertest;Password=dapperpass;Database=dappertest");
    }
    public class PostgresqlTests : TestBase<PostgresProvider>
    {
        private Npgsql.NpgsqlConnection GetOpenNpgsqlConnection() => (Npgsql.NpgsqlConnection)Provider.GetOpenConnection();

        private class Cat
        {
            public int Id { get; set; }
            public string? Breed { get; set; }
            public string? Name { get; set; }
        }

        private readonly Cat[] Cats =
        {
            new Cat() { Breed = "Abyssinian", Name="KACTUS"},
            new Cat() { Breed = "Aegean cat", Name="KADAFFI"},
            new Cat() { Breed = "American Bobtail", Name="KANJI"},
            new Cat() { Breed = "Balinese", Name="MACARONI"},
            new Cat() { Breed = "Bombay", Name="MACAULAY"},
            new Cat() { Breed = "Burmese", Name="MACBETH"},
            new Cat() { Breed = "Chartreux", Name="MACGYVER"},
            new Cat() { Breed = "German Rex", Name="MACKENZIE"},
            new Cat() { Breed = "Javanese", Name="MADISON"},
            new Cat() { Breed = "Persian", Name="MAGNA"}
        };

        
        

        private class CharTable
        {
            public int Id { get; set; }
            public char CharColumn { get; set; }
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class FactPostgresqlAttribute : FactAttribute
        {
            public override string? Skip
            {
                get { return unavailable ?? base.Skip; }
                set { base.Skip = value; }
            }

            private static readonly string? unavailable;

            static FactPostgresqlAttribute()
            {
                try
                {
                    using (DatabaseProvider<PostgresProvider>.Instance.GetOpenConnection()) { /* just trying to see if it works */ }
                }
                catch (Exception ex)
                {
                    unavailable = $"Postgresql is unavailable: {ex.Message}";
                }
            }
        }
    }
}
