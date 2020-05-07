﻿namespace MultiVendorRestaurantManagement.ApiContract.Request
{
    public class CreateAddOnRequest
    {
        public string Name { get; set; }
        public string NameEng { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string DescriptionEng { get; set; }
    }
}