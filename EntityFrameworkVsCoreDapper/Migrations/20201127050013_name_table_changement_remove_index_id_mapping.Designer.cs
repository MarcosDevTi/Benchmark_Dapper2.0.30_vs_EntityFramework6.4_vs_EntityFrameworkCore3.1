﻿// <auto-generated />
using System;
using EntityFrameworkVsCoreDapper.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityFrameworkVsCoreDapper.Migrations
{
    [DbContext(typeof(DotNetCoreContext))]
    [Migration("20201127050013_name_table_changement_remove_index_id_mapping")]
    partial class name_table_changement_remove_index_id_mapping
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("EntityFrameworkVsCoreDapper.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdministrativeRegion")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)")
                        .HasColumnName("administrative_region");

                    b.Property<string>("City")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)")
                        .HasColumnName("country");

                    b.Property<string>("Number")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("number");

                    b.Property<string>("Street")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("street");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("zip_code");

                    b.HasKey("Id");

                    b.ToTable("efdp_address");
                });

            modelBuilder.Entity("EntityFrameworkVsCoreDapper.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("address_id");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("birth_date");

                    b.Property<string>("Email")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)")
                        .HasColumnName("last_name");

                    b.Property<string>("Status")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("Email");

                    b.ToTable("efdp_customer");
                });

            modelBuilder.Entity("EntityFrameworkVsCoreDapper.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Brand")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("brand");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("customer_id");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("name");

                    b.Property<decimal>("OldPrice")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("old_price");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("price");

                    b.Property<Guid?>("ProductPageId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("product_page_id");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("Name");

                    b.HasIndex("ProductPageId");

                    b.ToTable("efdp_product");
                });

            modelBuilder.Entity("EntityFrameworkVsCoreDapper.ProductPage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullDescription")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("full_description");

                    b.Property<string>("ImageLink")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasColumnName("image_link");

                    b.Property<string>("SmallDescription")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)")
                        .HasColumnName("small_description");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("efdp_product_page");
                });

            modelBuilder.Entity("EntityFrameworkVsCoreDapper.Results.Result", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("OperationType")
                        .HasColumnType("int");

                    b.Property<double>("RamMax")
                        .HasColumnType("float");

                    b.Property<double>("RamMin")
                        .HasColumnType("float");

                    b.Property<TimeSpan>("TempoMax")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TempoMin")
                        .HasColumnType("time");

                    b.Property<int>("TypeTransaction")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("EntityFrameworkVsCoreDapper.Customer", b =>
                {
                    b.HasOne("EntityFrameworkVsCoreDapper.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("EntityFrameworkVsCoreDapper.Product", b =>
                {
                    b.HasOne("EntityFrameworkVsCoreDapper.Customer", "Customer")
                        .WithMany("Products")
                        .HasForeignKey("CustomerId");

                    b.HasOne("EntityFrameworkVsCoreDapper.ProductPage", "ProductPage")
                        .WithMany()
                        .HasForeignKey("ProductPageId");

                    b.Navigation("Customer");

                    b.Navigation("ProductPage");
                });

            modelBuilder.Entity("EntityFrameworkVsCoreDapper.Customer", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
