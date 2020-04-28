﻿using Common.Invariants;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using MultiVendorRestaurantManagement.Domain.City;
using MultiVendorRestaurantManagement.Domain.Common;
using MultiVendorRestaurantManagement.Domain.Foods;
using MultiVendorRestaurantManagement.Domain.Orders;
using MultiVendorRestaurantManagement.Domain.Restaurants;
using MultiVendorRestaurantManagement.Domain.ValueObjects;

namespace MultiVendorRestaurantManagement.Infrastructure
{
    public class RestaurantContext : DbContext
    {
        public DbSet<Domain.Restaurants.Restaurant> Restaurants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Manager> Managers { get; set; }

        public RestaurantContext(DbContextOptions<RestaurantContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>(builder =>
                {
                    builder.Property(x => x.Name).IsRequired();
                    builder.Property(x => x.IsVeg).IsRequired();
                    builder.Property(x => x.IsNonVeg).IsRequired();
                    builder.Property(x => x.IsGlutenFree).IsRequired();
                    builder.Property(x => x.ImageUrl).IsRequired();
                    
                    builder.Property(p => p.UnitPrice)
                        .IsRequired()
                        .HasConversion(p => p.Value, p => MoneyValue.Of(p));
                    builder.Property(p => p.OldUnitPrice)
                        .IsRequired()
                        .HasConversion(p => p.Value, p => MoneyValue.Of(p));
                    builder.Property(x => x.Type)
                        .IsRequired()
                        .HasConversion<string>();
                    builder.Property(x => x.Status)
                        .IsRequired()
                        .HasConversion<string>();

                    builder.HasMany(x => x.Categories)
                        .WithOne()
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    builder.Metadata
                        .FindNavigation("Categories")
                        .SetPropertyAccessMode(PropertyAccessMode.Field);

                    builder.HasMany(x => x.Variants)
                        .WithOne(v => v.Food)
                        .OnDelete(DeleteBehavior.Cascade);
                    builder.Metadata
                        .FindNavigation("Variants")
                        .SetPropertyAccessMode(PropertyAccessMode.Field);

                    builder.HasMany(x => x.AddOns)
                        .WithOne(v => v.Food)
                        .OnDelete(DeleteBehavior.Cascade);

                    builder.Metadata
                        .FindNavigation("AddOns")
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
                }
            );

            modelBuilder.Entity<Variant>(builder =>
            {
                builder.Property(x => x.Price)
                    .IsRequired()
                    .HasConversion(p => p.Value, p => MoneyValue.Of(p));
                builder.Property(x => x.Name)
                    .IsRequired();
                builder.Property(x => x.NameEng)
                    .IsRequired();
            });

            modelBuilder.Entity<Order>(builder =>
            {
                builder.HasMany(x => x.Items)
                    .WithOne(v => v.Order)
                    .OnDelete(DeleteBehavior.Cascade)
                    .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
                builder.HasOne(x => x.Detail);
                builder.Property(x => x.State)
                    .IsRequired()
                    .HasConversion<string>();
                builder.Property(x => x.Type)
                    .IsRequired()
                    .HasConversion<string>();
                builder.Property(x => x.PaymentType)
                    .IsRequired()
                    .HasConversion<string>();
                builder.Property(p => p.PayableAmount)
                    .IsRequired()
                    .HasConversion(p => p.Value, p => MoneyValue.Of(p));
                builder.Property(p => p.TotalAmount)
                    .IsRequired()
                    .HasConversion(p => p.Value, p => MoneyValue.Of(p));
            });

            modelBuilder.Entity<OrderDetail>(builder =>
            {
                builder.Property(x => x.Address)
                    .IsRequired();
                builder.Property(x => x.CustomerName)
                    .IsRequired();
                builder.Property(x => x.ContactNumber)
                    .IsRequired();
                builder.Property(x => x.DeliveryLocation)
                    .IsRequired()
                    .HasConversion(x => $"{x.Latitude},{x.Longitude}", x => new LocationValue(x));
            });

            modelBuilder.Entity<City>(builder =>
            {
                builder.Property(x => x.Name)
                    .IsRequired();
                builder.Property(x => x.NameEng)
                    .IsRequired();
                builder.Property(x => x.Code)
                    .IsRequired();
                
                builder.HasMany(x => x.Localities)
                    .WithOne(x => x.City)
                    .OnDelete(DeleteBehavior.Cascade)
                    .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Category>(builder =>
            {
                builder.Property(x => x.Name)
                    .IsRequired();
                builder.Property(x => x.NameEng)
                    .IsRequired();
                builder.Property(x => x.ImageUrl)
                    .IsRequired();
            });

            modelBuilder.Entity<Locality>(builder =>
            {
                builder.Property(x => x.Name)
                    .IsRequired();
                
                builder.Property(x => x.NameEng)
                    .IsRequired();

                builder.Property(x => x.Code)
                    .IsRequired();

            });

            modelBuilder.Entity<Domain.Restaurants.Restaurant>(builder =>
                {
                    builder.Property(x => x.Name)
                        .IsRequired();
                    builder.Property(x => x.PhoneNumberNumber)
                        .IsRequired()
                        .HasConversion(x => x.GetCompletePhoneNumber(),
                            p => PhoneNumberValue.Of(SupportedCountryCode.Italy, p));
                    builder.HasOne(x => x.Locality)
                        .WithMany()
                        .IsRequired();
                    builder.HasOne(x => x.Manager);
                    builder.Property(x => x.OpeningHour)
                        .IsRequired();
                    builder.Property(x => x.ClosingHour)
                        .IsRequired();
                    builder.Property(x => x.State)
                        .IsRequired()
                        .HasConversion<string>();

                    builder.Property(x => x.SubscriptionType)
                        .IsRequired()
                        .HasConversion<string>();
                    builder.Property(x => x.ContractStatus)
                        .IsRequired()
                        .HasConversion<string>();

                    builder.HasOne(x => x.PricingPolicy);

                    builder.HasMany(x => x.Foods)
                        .WithOne(f => f.Restaurant)
                        .OnDelete(DeleteBehavior.Cascade);

                    builder.Metadata
                        .FindNavigation("Foods")
                        .SetPropertyAccessMode(PropertyAccessMode.Field);

                    builder.HasMany(x => x.Menus)
                        .WithOne(f => f.Restaurant)
                        .OnDelete(DeleteBehavior.Cascade);
                    builder.Metadata
                        .FindNavigation("Menus")
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
                    builder.HasMany(x => x.Orders)
                        .WithOne(f => f.Restaurant)
                        .OnDelete(DeleteBehavior.Cascade);
                    builder.Metadata
                        .FindNavigation("Orders")
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
                }
            );

            modelBuilder.Entity<Menu>(buider =>
            {
                buider.Property(x => x.Name).IsRequired();
            });
            modelBuilder.Entity<Promotion>(builder =>
            {
                builder.Property(x => x.StartDate)
                    .IsRequired();
                builder.Property(x => x.Description)
                    .IsRequired();
                builder.Property(x => x.ImageUrl)
                    .IsRequired();
                builder.Property(x => x.EndDate)
                    .IsRequired();
                builder.HasMany(x => x.Items)
                    .WithOne(x => x.Promotion)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired()
                    .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
                
            });

            modelBuilder.Entity<Review>(builder =>
                {
                    builder.Property(x => x.StarRate)
                        .IsRequired();
                    builder.Property(x => x.UserPhoneNumber)
                        .IsRequired();
                    builder.Property(x => x.ItemId)
                        .IsRequired();
                    builder.Property(x => x.UserPhoneNumber)
                        .HasConversion(x => x.GetCompletePhoneNumber(),
                            p => PhoneNumberValue.Of(SupportedCountryCode.Italy, p));
                }
            );
            modelBuilder.Entity<AddOn>(builder =>
            {
                builder.Property(x => x.Name)
                    .IsRequired();
                
                builder.Property(x => x.Description)
                    .IsRequired();
                
                builder.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,4)");
            });

            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.Property(x => x.Quantity)
                    .IsRequired();
                builder.Property(x => x.FoodId)
                    .IsRequired();
                builder.Property(x => x.FoodName)
                    .IsRequired();
                builder.Property(p => p.Total)
                    .IsRequired()
                    .HasColumnType("decimal(18,4)");
                builder.Property(p => p.Discount)
                    .HasColumnType("decimal(18,4)");
            });
            modelBuilder.Entity<PricingPolicy>(x =>
            {
                x.Property(p => p.MinimumCharge)
                    .IsRequired()
                    .HasColumnType("decimal(18,4)");
                x.Property(p => p.MaximumCharge)
                    .HasColumnType("decimal(18,4)");
                x.Property(p => p.FixedCharge).IsRequired()
                    .HasColumnType("decimal(18,4)");
                x.Property(p => p.AdditionalPrice)
                    .HasColumnType("decimal(18,4)");
            });

            modelBuilder.Entity<Tag>(builder =>
            {
                builder.Property(x => x.Name)
                    .IsRequired();
                
                builder.Property(x => x.NameEng)
                    .IsRequired();
                
            });

            modelBuilder.Entity<FoodCategory>(builder =>
            {
                builder.HasKey(x => new {x.CategoryId, x.FoodId});
                builder.HasOne(x => x.Food)
                    .WithMany(x => x.Categories)
                    .HasForeignKey(x=>x.FoodId);
                builder.HasOne(x => x.Category)
                    .WithMany(x => x.Foods)
                    .HasForeignKey(x=>x.CategoryId);
                
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}