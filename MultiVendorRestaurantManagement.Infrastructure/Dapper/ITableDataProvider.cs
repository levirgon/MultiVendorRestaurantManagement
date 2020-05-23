﻿using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<DealTableData> GetDealAsync(string notificationDealName);
        Task<List<CuisineTableDataMinimal>> GetCuisineListAsync(IEnumerable<long> cuisineIds);
        Task<List<CategoryTableDataMinimal>> GetCategoryListAsync(IEnumerable<long> categoryIds);
    }
}