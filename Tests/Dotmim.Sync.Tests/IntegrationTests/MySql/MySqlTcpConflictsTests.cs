﻿using Dotmim.Sync.MariaDB;
using Dotmim.Sync.MySql;
using Dotmim.Sync.PostgreSql;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.SqlServer;
using Dotmim.Sync.Tests.Core;
using Dotmim.Sync.Tests.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Dotmim.Sync.Tests.IntegrationTests
{
    public class MySqlTcpConflictsTests : TcpConflictsTests
    {
        public override List<ProviderType> ClientsType => new List<ProviderType>
            { ProviderType.MySql,  ProviderType.Sqlite};

        public MySqlTcpConflictsTests(HelperProvider fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        public override string[] Tables => new string[]
        {
            "ProductCategory", "ProductModel", "Product", "Employee", "Customer", "Address", "CustomerAddress", "EmployeeAddress",
            "SalesOrderHeader", "SalesOrderDetail", "Posts", "Tags", "PostTag",
            "PricesList", "PricesListCategory", "PricesListDetail", "Log"
        };

        public override ProviderType ServerType =>
            ProviderType.MySql;





        /// <summary>
        /// Get the server database rows count
        /// </summary>
        /// <returns></returns>
        public override int GetServerDatabaseRowsCount((string DatabaseName, ProviderType ProviderType, CoreProvider Provider) t)
        {
            int totalCountRows = 0;

            using (var serverDbCtx = new AdventureWorksContext(t, false))
            {
                totalCountRows += serverDbCtx.Address.Count();
                totalCountRows += serverDbCtx.Customer.Count();
                totalCountRows += serverDbCtx.CustomerAddress.Count();
                totalCountRows += serverDbCtx.Employee.Count();
                totalCountRows += serverDbCtx.EmployeeAddress.Count();
                totalCountRows += serverDbCtx.Log.Count();
                totalCountRows += serverDbCtx.Posts.Count();
                totalCountRows += serverDbCtx.PostTag.Count();
                totalCountRows += serverDbCtx.PricesList.Count();
                totalCountRows += serverDbCtx.PricesListCategory.Count();
                totalCountRows += serverDbCtx.PricesListDetail.Count();
                totalCountRows += serverDbCtx.Product.Count();
                totalCountRows += serverDbCtx.ProductCategory.Count();
                totalCountRows += serverDbCtx.ProductModel.Count();
                totalCountRows += serverDbCtx.SalesOrderDetail.Count();
                totalCountRows += serverDbCtx.SalesOrderHeader.Count();
                //totalCountRows += serverDbCtx.Sql.Count();
                totalCountRows += serverDbCtx.Tags.Count();
            }

            return totalCountRows;
        }

        public override async Task EnsureDatabaseSchemaAndSeedAsync((string DatabaseName, ProviderType ProviderType, CoreProvider Provider) t, bool useSeeding = false, bool useFallbackSchema = false)
        {
            AdventureWorksContext ctx = null;
            try
            {
                ctx = new AdventureWorksContext(t, useFallbackSchema, useSeeding);
                await ctx.Database.EnsureCreatedAsync();

            }
            catch (Exception)
            {
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }
        }

        public override Task CreateDatabaseAsync(ProviderType providerType, string dbName, bool recreateDb = true)
        {
            return HelperDatabase.CreateDatabaseAsync(providerType, dbName, recreateDb);
        }

    }
}
