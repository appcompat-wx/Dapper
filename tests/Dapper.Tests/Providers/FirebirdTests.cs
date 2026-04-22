using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace Dapper.Tests.Providers
{
    /// <summary>
    /// If Docker Desktop is installed, run the following command to start a container suitable for the tests.
    /// <code>
    /// docker run -d -p 3050:3050 --name Dapper.Tests.Firebird -e FIREBIRD_DATABASE=database -e ISC_PASSWORD=masterkey jacobalberty/firebird
    /// </code>
    /// </summary>
    public class FirebirdProvider : DatabaseProvider
    {
        public override DbProviderFactory Factory => FirebirdClientFactory.Instance;
        public override string GetConnectionString() => "initial catalog=localhost:database;user id=SYSDBA;password=masterkey";
    }
    public class FirebirdTests : TestBase<FirebirdProvider>
    {
        private FbConnection GetOpenFirebirdConnection() => (FbConnection)Provider.GetOpenConnection();

        
    }
}
