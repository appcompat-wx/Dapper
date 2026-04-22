#if !NETFRAMEWORK // platform not supported exception
using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Data.Client;
using Xunit;
using Xunit.Abstractions;

namespace Dapper.Tests
{
    public class SnowflakeTests
    {
        static readonly string? s_ConnectionString;
        static SnowflakeTests()
        {
            SqlMapper.Settings.UseIncrementalPseudoPositionalParameterNames = true;

            try
            { // this *probably* won't exist (TODO: can we get a test account?)
                s_ConnectionString = File.ReadAllText(@"c:\Code\SnowflakeConnectionString.txt").Trim();
            } catch { }
        }

        public SnowflakeTests(ITestOutputHelper output)
            => Output = output;

        private ITestOutputHelper Output { get; }

        
        private static SnowflakeDbConnection GetConnection()
        {
            if (string.IsNullOrWhiteSpace(s_ConnectionString))
                Skip.Inconclusive("no snowflake connection-string");

            return new SnowflakeDbConnection
            {
                ConnectionString = s_ConnectionString
            };
        }

        


        

        public class Nation
        {
            public int N_NATIONKEY { get; set; }
            public string? N_NAME{ get; set; }
            public int N_REGIONKEY { get; set; }
            public string? N_COMMENT { get; set; }
        }
    }
}
#endif
