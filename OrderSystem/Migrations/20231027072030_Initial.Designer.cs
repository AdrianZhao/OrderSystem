﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderSystem.Data;

#nullable disable

namespace OrderSystem.Migrations
{
    [DbContext(typeof(OrderSystemContext))]
    [Migration("20231027072030_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OrderSystem.Models.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("OrderSystem.Models.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ConversionRate")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");

                    b.HasData(
                        new
                        {
                            Id = new Guid("40bd6510-aea1-4af0-8366-55a5939804dc"),
                            ConversionRate = 1.0m,
                            Name = "Canada"
                        },
                        new
                        {
                            Id = new Guid("d58ab7b8-53ff-446d-8fc1-9a809ec6ac7a"),
                            ConversionRate = 0.73m,
                            Name = "USA"
                        },
                        new
                        {
                            Id = new Guid("8d1d5eaf-3e84-4db6-9b75-b1657c64b79d"),
                            ConversionRate = 13.29m,
                            Name = "Mexico"
                        });
                });

            modelBuilder.Entity("OrderSystem.Models.InCartProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("InCartProducts");
                });

            modelBuilder.Entity("OrderSystem.Models.InOrderProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("InOrderProducts");
                });

            modelBuilder.Entity("OrderSystem.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DestinationCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MailingCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OrderSystem.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AvailableQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("13208e45-595f-45c2-920b-ca10a95e7c8a"),
                            AvailableQuantity = 1,
                            Description = "XiaoGou's favorite",
                            Name = "Cat toy",
                            Price = 10.99m
                        },
                        new
                        {
                            Id = new Guid("10d9b8b8-3bab-46ca-bf6a-7f5a54ab7ac1"),
                            AvailableQuantity = 5,
                            Description = "NuoMi's favorite",
                            Name = "Cat licking toy",
                            Price = 19.99m
                        },
                        new
                        {
                            Id = new Guid("94ccd15a-48ae-4de3-991c-6cad55a52caf"),
                            AvailableQuantity = 2,
                            Description = "ZhaZha's favorite",
                            Name = "Cat tree",
                            Price = 9.99m
                        },
                        new
                        {
                            Id = new Guid("75c0a25e-29bd-4613-90e6-8a9fc9384538"),
                            AvailableQuantity = 0,
                            Description = "Best cat food",
                            Name = "Cat food",
                            Price = 4.99m
                        },
                        new
                        {
                            Id = new Guid("e4bd5a9e-eaf9-44ce-9067-283711181bae"),
                            AvailableQuantity = 3,
                            Description = "Best Cat scratch post",
                            Name = "Cat scratch post",
                            Price = 12.99m
                        });
                });

            modelBuilder.Entity("OrderSystem.Models.InCartProduct", b =>
                {
                    b.HasOne("OrderSystem.Models.Cart", null)
                        .WithMany("Products")
                        .HasForeignKey("CartId");

                    b.HasOne("OrderSystem.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OrderSystem.Models.InOrderProduct", b =>
                {
                    b.HasOne("OrderSystem.Models.Order", null)
                        .WithMany("Products")
                        .HasForeignKey("OrderId");

                    b.HasOne("OrderSystem.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OrderSystem.Models.Cart", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("OrderSystem.Models.Order", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
