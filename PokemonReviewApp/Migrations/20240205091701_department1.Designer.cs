﻿// <auto-generated />
using System;
using API_Dinamis.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240205091701_department1")]
    partial class department1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PokemonReviewApp.Models.Authx", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Auths");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Address2")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BranchCode")
                        .IsRequired()
                        .HasColumnType("varchar(6)");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("date");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ZipCodeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("ZipCodeId");

                    b.ToTable("Branch");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.BranchLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<string>("ActionRemarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Address2")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BranchCode")
                        .IsRequired()
                        .HasColumnType("varchar(6)");

                    b.Property<int>("BranchId")
                        .HasColumnType("integer");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("BranchTempId")
                        .HasColumnType("integer");

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("date");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ZipCodeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("BranchLog");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.BranchLogTemp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<string>("ActionRemarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Address2")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BranchCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BranchId")
                        .HasColumnType("integer");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("date");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ZipCodeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("BranchLogTemp");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CityCode")
                        .IsRequired()
                        .HasColumnType("varchar(6)");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("City");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<int>("BranchId")
                        .HasColumnType("integer");

                    b.Property<string>("DepartmentCode")
                        .IsRequired()
                        .HasColumnType("varchar(6)");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.DepartmentLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<string>("ActionRemarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("BranchId")
                        .HasColumnType("integer");

                    b.Property<string>("DepartmentCode")
                        .IsRequired()
                        .HasColumnType("varchar(6)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("DepartmentTempId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DepartmentLog");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.DepartmentLogTemp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.Property<string>("ActionRemarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("BranchId")
                        .HasColumnType("integer");

                    b.Property<string>("DepartmentCode")
                        .IsRequired()
                        .HasColumnType("varchar(6)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DepartmentLogTemp");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.Key", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("Keys")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Key");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.Pokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Pokemons");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.Zipcode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("ZipCode");
                });

            modelBuilder.Entity("PokemonReviewApp.Models.Branch", b =>
                {
                    b.HasOne("PokemonReviewApp.Models.City", null)
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PokemonReviewApp.Models.Zipcode", null)
                        .WithMany()
                        .HasForeignKey("ZipCodeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PokemonReviewApp.Models.Department", b =>
                {
                    b.HasOne("PokemonReviewApp.Models.Branch", null)
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PokemonReviewApp.Models.Zipcode", b =>
                {
                    b.HasOne("PokemonReviewApp.Models.City", null)
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
