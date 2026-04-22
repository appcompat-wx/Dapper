using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dapper.Tests
{
    /// <summary>
    /// If Docker Desktop is installed, run the following command to start a container suitable for the tests.
    /// <code>
    /// docker run -d -p 3306:3306 --name Dapper.Tests.MySQL -e MYSQL_DATABASE=tests -e MYSQL_USER=test -e MYSQL_PASSWORD=pass -e MYSQL_ROOT_PASSWORD=pass mysql
    /// </code>
    /// </summary>
    public sealed class MySqlProvider : DatabaseProvider
    {
        public override DbProviderFactory Factory => MySqlConnector.MySqlConnectorFactory.Instance;
        public override string GetConnectionString() =>
            GetConnectionString("MySqlConnectionString", "Server=localhost;Database=tests;Uid=test;Pwd=pass;");

        public DbConnection GetMySqlConnection(bool open = true,
            bool convertZeroDatetime = false, bool allowZeroDatetime = false)
        {
            string cs = GetConnectionString();
            var csb = Factory.CreateConnectionStringBuilder()!;
            csb.ConnectionString = cs;
            ((dynamic)csb).AllowZeroDateTime = allowZeroDatetime;
            ((dynamic)csb).ConvertZeroDateTime = convertZeroDatetime;
            var conn = Factory.CreateConnection()!;
            conn.ConnectionString = csb.ConnectionString;
            if (open) conn.Open();
            return conn;
        }
    }
    public class MySQLTests : TestBase<MySqlProvider>
    {      
        

        private class SO36303462
        {
            public int Id { get; set; }
            public bool IsBold { get; set; }
        }

        public class Issue426_Test
        {
            public long Id { get; set; }
            public TimeSpan? Time { get; set; }
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class FactMySqlAttribute : FactAttribute
        {
            public override string? Skip
            {
                get { return unavailable ?? base.Skip; }
                set { base.Skip = value; }
            }

            private static readonly string? unavailable;

            static FactMySqlAttribute()
            {
                try
                {
                    using (DatabaseProvider<MySqlProvider>.Instance.GetMySqlConnection(true)) { /* just trying to see if it works */ }
                }
                catch (Exception ex)
                {
                    unavailable = $"MySql is unavailable: {ex.Message}";
                }
            }
        }
    }
}
