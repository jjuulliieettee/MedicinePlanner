﻿// <auto-generated />
using System;
using MedicinePlanner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedicinePlanner.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20201111161936_UpdatedUserModel")]
    partial class UpdatedUserModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MedicinePlanner.Data.Models.FoodRelation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FoodRelations");
                });

            modelBuilder.Entity("MedicinePlanner.Data.Models.FoodSchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("MedicineScheduleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NumberOfMeals")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TimeOfFirstMeal")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("MedicineScheduleId");

                    b.ToTable("FoodSchedules");
                });

            modelBuilder.Entity("MedicinePlanner.Data.Models.Medicine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActiveSubstance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Dosage")
                        .HasColumnType("int");

                    b.Property<int>("FoodInterval")
                        .HasColumnType("int");

                    b.Property<Guid>("FoodRelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfTakes")
                        .HasColumnType("int");

                    b.Property<Guid>("PharmaceuticalFormId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FoodRelationId");

                    b.HasIndex("PharmaceuticalFormId");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("MedicinePlanner.Data.Models.MedicineSchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("MedicineId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.HasIndex("UserId");

                    b.ToTable("MedicineSchedules");
                });

            modelBuilder.Entity("MedicinePlanner.Data.Models.PharmaceuticalForm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PharmaceuticalForms");
                });

            modelBuilder.Entity("MedicinePlanner.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MedicinePlanner.Data.Models.FoodSchedule", b =>
                {
                    b.HasOne("MedicinePlanner.Data.Models.MedicineSchedule", "MedicineSchedule")
                        .WithMany("FoodSchedules")
                        .HasForeignKey("MedicineScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicinePlanner.Data.Models.Medicine", b =>
                {
                    b.HasOne("MedicinePlanner.Data.Models.FoodRelation", "FoodRelation")
                        .WithMany("Medicine")
                        .HasForeignKey("FoodRelationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicinePlanner.Data.Models.PharmaceuticalForm", "PharmaceuticalForm")
                        .WithMany("Medicine")
                        .HasForeignKey("PharmaceuticalFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicinePlanner.Data.Models.MedicineSchedule", b =>
                {
                    b.HasOne("MedicinePlanner.Data.Models.Medicine", "Medicine")
                        .WithMany("MedicineSchedules")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicinePlanner.Data.Models.User", "User")
                        .WithMany("MedicineSchedules")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
