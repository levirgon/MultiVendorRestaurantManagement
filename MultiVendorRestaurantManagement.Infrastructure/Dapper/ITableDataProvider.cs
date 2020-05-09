﻿using System.Threading.Tasks;
using MultiVendorRestaurantManagement.Infrastructure.Dapper.DbView;
using MultiVendorRestaurantManagement.Infrastructure.Dapper.TableData;

namespace MultiVendorRestaurantManagement.Infrastructure.Dapper
{
    public interface ITableDataProvider
    {
        Task<LocalityTableData> GetLocalityAsync(long cityId, string localityName);
        Task<CityTableData> GetCityAsync(string name);
        Task<CategoryTableData> GetCategoryAsync(string name);
        Task<RestaurantTableData> GetRestaurantAsync(string phone);
        Task<MenuTableData2> GetMenuAsync(string menuName);
        Task<FoodTableData> GetFoodAsync(long restaurantId, string foodName);
    }
}