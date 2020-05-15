﻿using System;
using System.Collections.Generic;
using Common.Invariants;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiVendorRestaurantManagement.Infrastructure.Mongo.Documents
{
    public class RestaurantDocument : BaseDocument
    {
        public RestaurantDocument(long restaurantId, string name, string phoneNumber, long localityId, string state,
            int openingHour, int closingHour, string subscriptionType, string contractStatus, string imageUrl,
            double rating, int totalRatingsCount, long categoryId, string categoryName, DateTime expirationDate, string description, string descriptionEng)
        {
            RestaurantId = restaurantId;
            Name = name;
            PhoneNumber = phoneNumber;
            LocalityId = localityId;
            State = state;
            OpeningHour = openingHour;
            ClosingHour = closingHour;
            SubscriptionType = subscriptionType;
            ContractStatus = contractStatus;
            ImageUrl = imageUrl;
            Rating = rating;
            TotalRatingsCount = totalRatingsCount;
            CategoryId = categoryId;
            CategoryName = categoryName;
            ExpirationDate = expirationDate;
            Description = description;
            DescriptionEng = descriptionEng;
            Menus = new List<MenuRecord>();
        }
        public string Description { get;protected set; }
        public string DescriptionEng { get;protected set; }
        public long RestaurantId { get; protected set; }
        public string Name { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public long LocalityId { get; protected set; }
        public string State { get; protected set; }
        public int OpeningHour { get; protected set; }
        public int ClosingHour { get; protected set; }
        public string SubscriptionType { get; protected set; }
        public string ContractStatus { get; protected set; }
        public string ImageUrl { get; protected set; }
        public double Rating { get; protected set; }
        public int TotalRatingsCount { get; protected set; }
        public List<MenuRecord> Menus { get; protected set;}
        public long ManagerId { get; protected set; }
        public long PricingPolicyId { get; protected set; }
        public long CategoryId { get; private set; }
        public string CategoryName { get; protected set;  }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExpirationDate { get; private set; }

        public void UpdateHours(in int openingHour, in int closingHour)
        {
            OpeningHour = openingHour;
            ClosingHour = closingHour;
        }

        public void UpdateSubscription(SubscriptionType subscription)
        {
            SubscriptionType = subscription.ToString();
            ExpirationDate = subscription.GetExpirationTime();
        }

        public void UpdateCategory(in long categoryId)
        {
            CategoryId = categoryId;
        }

        public void AddNewMenu(MenuRecord menu)
        {
            Menus.Add(menu);
        }
    }

    public class MenuRecord
    {
        public MenuRecord(long menuId, string name, string nameEng, string imageUrl)
        {
            MenuId = menuId;
            Name = name;
            NameEng = nameEng;
            ImageUrl = imageUrl;
        }

        public long MenuId { get; protected set; }
        public string Name { get; protected set; }
        public string NameEng { get; protected set; }
        public string ImageUrl { get; protected set; }
    }
}