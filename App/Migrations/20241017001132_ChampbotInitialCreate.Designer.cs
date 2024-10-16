﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20241017001132_ChampbotInitialCreate")]
    partial class ChampbotInitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("champbot")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Database.Entity.EventEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EventTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Event", "champbot");
                });

            modelBuilder.Entity("Database.Entity.ReminderEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Reminded")
                        .HasColumnType("boolean");

                    b.Property<string>("ReminderNote")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("TimeBeforeEvent")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Reminder", "champbot");
                });

            modelBuilder.Entity("Database.Entity.ReminderEntity", b =>
                {
                    b.HasOne("Database.Entity.EventEntity", "Event")
                        .WithMany("Reminders")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Database.Entity.EventEntity", b =>
                {
                    b.Navigation("Reminders");
                });
#pragma warning restore 612, 618
        }
    }
}
