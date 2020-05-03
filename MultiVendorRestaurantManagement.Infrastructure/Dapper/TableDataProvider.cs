﻿using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MultiVendorRestaurantManagement.Domain.Cities;
using MultiVendorRestaurantManagement.Infrastructure.Dapper.DbView;
using MultiVendorRestaurantManagement.Infrastructure.Dapper.TableData;

namespace MultiVendorRestaurantManagement.Infrastructure.Dapper
{
    public class TableDataProvider : ITableDataProvider
    {
        private string _connectionString;

        public TableDataProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<LocalityTableData> GetLocalityAsync(long cityId, string localityName)
        {
            const string sql = "SELECT * FROM Locality WHERE (Name = @Name AND CityId = @CityId)";
            await using var connection = new SqlConnection(_connectionString);
            var locality =
                await connection.QueryFirstOrDefaultAsync<LocalityTableData>(sql,
                    new {Name = localityName, CityId = cityId});
            return locality;
        }

        public async Task<CityTableData> GetCityAsync(string name)
        {
            const string sql = "SELECT * FROM Cities WHERE Name = @Name";
            await using var connection = new SqlConnection(_connectionString);
            var city =
                await connection.QueryFirstOrDefaultAsync<CityTableData>(sql,
                    new {Name = name});
            return city;
        }

        public async Task<CategoryTableData> GetCategoryAsync(string name)
        {
            const string sql = "select * from Categories where Name = @Name";
            await using var connection = new SqlConnection(_connectionString);
            var category =
                await connection.QueryFirstOrDefaultAsync<CategoryTableData>(sql,
                    new {Name = name});
            return category;
        }
    }
}