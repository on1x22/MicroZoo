﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using MicroZoo.ZookeepersApi.DBContext;

#nullable disable

namespace MicroZoo.Infrastructure.Migrations
{
    [DbContext(typeof(ZookeeperDBContext))]
    [Migration("20230508184106_CreateJobsAndSpecialitiesDatabases")]
    partial class CreateJobsAndSpecialitiesDatabases
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MicroZoo.ZookeepersApi.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("description");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("finishtime");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("starttime");

                    b.Property<int>("ZookeeperId")
                        .HasColumnType("integer")
                        .HasColumnName("zookeeperid");

                    b.HasKey("Id");

                    b.ToTable("jobs");
                });

            modelBuilder.Entity("MicroZoo.ZookeepersApi.Models.Speciality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("animaltypeid");

                    b.Property<int>("ZookeeperId")
                        .HasColumnType("integer")
                        .HasColumnName("zookeeperid");

                    b.HasKey("Id");

                    b.ToTable("specialities");
                });

            modelBuilder.Entity("MicroZoo.ZookeepersApi.Models.Zookepeer", b =>
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
