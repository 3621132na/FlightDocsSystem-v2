﻿// <auto-generated />
using System;
using FlightService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlightService.Migrations
{
    [DbContext(typeof(FlightContext))]
    [Migration("20240525224327_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FlightService.Models.Flight", b =>
                {
                    b.Property<int>("FlightID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlightID"), 1L, 1);

                    b.Property<string>("AircraftType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ArrivalAirportID")
                        .HasColumnType("int");

                    b.Property<string>("ArrivalAirportName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DepartureAirportID")
                        .HasColumnType("int");

                    b.Property<string>("DepartureAirportName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReturnedFiles")
                        .HasColumnType("int");

                    b.Property<int>("SentFiles")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("FlightID");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("FlightService.Models.UserFlight", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("FlightID")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID", "FlightID");

                    b.HasIndex("FlightID");

                    b.ToTable("UserFlights");
                });

            modelBuilder.Entity("FlightService.Models.UserFlight", b =>
                {
                    b.HasOne("FlightService.Models.Flight", null)
                        .WithMany("Users")
                        .HasForeignKey("FlightID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlightService.Models.Flight", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
