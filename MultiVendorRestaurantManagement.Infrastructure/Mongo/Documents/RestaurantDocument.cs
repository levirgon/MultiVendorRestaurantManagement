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
            double rating, int totalRatingsCount, long categoryId, DateTime expirationDate)
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
            ExpirationDate = expirationDate;
            Menus = new List<MenuRecord>();
        }

        public long RestaurantId { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public long LocalityId { get; private set; }
        public string State { get; protected set; }
        public int OpeningHour { get; protected set; }
        public int ClosingHour { get; protected set; }
        public string SubscriptionType { get; protected set; }
        public string ContractStatus { get; protected set; }
        public string ImageUrl { get; private set; }
        public double Rating { get; private set; }
        public int TotalRatingsCount { get; private set; }
        public List<MenuRecord> Menus { get; private set; }
        public long ManagerId { get; set; }
        public long PricingPolicyId { get; set; }
        public long CategoryId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime ExpirationDate { get; set; }
    }

    public class MenuRecord
    {
        public long MenuId { get; private set; }
        public string Name { get; private set; }
        public string NameEng { get; protected set; }
    }
}