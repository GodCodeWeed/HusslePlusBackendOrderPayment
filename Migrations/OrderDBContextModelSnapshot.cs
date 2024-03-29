﻿// <auto-generated />
using System;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Migrations
{
    [DbContext(typeof(OrderDBContext))]
    partial class OrderDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.OrderedProduct", b =>
                {
                    b.Property<string>("Order_ProductID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Order_ID")
                        .HasColumnType("int");

                    b.Property<string>("Order_ProductCost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order_ProductQuantity")
                        .HasColumnType("int");

                    b.HasKey("Order_ProductID");

                    b.HasIndex("Order_ID");

                    b.ToTable("OrderedProduct");
                });

            modelBuilder.Entity("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.OrdredProducts", b =>
                {
                    b.Property<int>("Order_ID")
                        .HasColumnType("int");

                    b.Property<string>("Order_SellerID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Order_TotalCost")
                        .HasColumnType("real");

                    b.HasKey("Order_ID");

                    b.ToTable("OrdredProducts");
                });

            modelBuilder.Entity("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.Request", b =>
                {
                    b.Property<int>("Order_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Buyer_ID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Order_Cost")
                        .HasColumnType("real");

                    b.Property<string>("Order_CustomersName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Order_DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Order_Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Order_ShippingAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order_State")
                        .HasColumnType("int");

                    b.Property<string>("Order_Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Order_ID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.OrderedProduct", b =>
                {
                    b.HasOne("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.OrdredProducts", null)
                        .WithMany("Order_Products")
                        .HasForeignKey("Order_ID");
                });

            modelBuilder.Entity("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.OrdredProducts", b =>
                {
                    b.HasOne("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.Request", null)
                        .WithMany("Order_Product")
                        .HasForeignKey("Order_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.OrdredProducts", b =>
                {
                    b.Navigation("Order_Products");
                });

            modelBuilder.Entity("HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities.Request", b =>
                {
                    b.Navigation("Order_Product");
                });
#pragma warning restore 612, 618
        }
    }
}
