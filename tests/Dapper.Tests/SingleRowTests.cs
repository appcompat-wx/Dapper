using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastMember;
using Xunit;
using Xunit.Abstractions;
using static Dapper.SqlMapper;

namespace Dapper.Tests;

[Collection("SingleRowTests")]
public sealed class SystemSqlClientSingleRowTests(ITestOutputHelper log) : SingleRowTests<SystemSqlClientProvider>(log)
{
    protected override async Task InjectDataAsync(DbConnection conn, DbDataReader source)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        using var bcp = new System.Data.SqlClient.SqlBulkCopy((System.Data.SqlClient.SqlConnection)conn);
#pragma warning restore CS0618 // Type or member is obsolete
        bcp.DestinationTableName = "#mydata";
        bcp.EnableStreaming = true;
        await bcp.WriteToServerAsync(source);
    }
}
#if MSSQLCLIENT
[Collection("SingleRowTests")]
public sealed class MicrosoftSqlClientSingleRowTests(ITestOutputHelper log) : SingleRowTests<MicrosoftSqlClientProvider>(log)
{
    protected override async Task InjectDataAsync(DbConnection conn, DbDataReader source)
    {
        using var bcp = new Microsoft.Data.SqlClient.SqlBulkCopy((Microsoft.Data.SqlClient.SqlConnection)conn);
        bcp.DestinationTableName = "#mydata";
        bcp.EnableStreaming = true;
        await bcp.WriteToServerAsync(source);
    }
}
#endif
public abstract class SingleRowTests<TProvider>(ITestOutputHelper log) : TestBase<TProvider> where TProvider : DatabaseProvider
{
    protected abstract Task InjectDataAsync(DbConnection connection, DbDataReader source);

    

    public class MyRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}

internal static class AsyncLinqHelper
{
    public static async ValueTask<T> FirstAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        await using var iter = source.GetAsyncEnumerator(cancellationToken);
        if (!await iter.MoveNextAsync()) Array.Empty<T>().First(); // for consistent error
        return iter.Current;
    }
}
