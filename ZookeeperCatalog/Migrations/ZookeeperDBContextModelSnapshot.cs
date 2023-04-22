﻿// <auto-generated />
using System.Collections.Generic;
using MicroZoo.ZookeeperCatalog.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZookeeperCatalog.Migrations
{
    [DbContext(typeof(ZookeeperDBContext))]
    partial class ZookeeperDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MicroZoo.ZookeeperCatalog.Models.Zookepeer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<List<string>>("Specialities")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("specialities");

                    b.HasKey("Id");

                    b.ToTable("zookeeper");
                });
#pragma warning restore 612, 618
        }
    }
}
