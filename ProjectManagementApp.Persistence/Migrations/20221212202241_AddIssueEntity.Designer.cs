﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectManagementApp.Persistence;

#nullable disable

namespace ProjectManagementApp.Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20221212202241_AddIssueEntity")]
    partial class AddIssueEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.EmployeeProject", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("EmployeeProject");
                });

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AssigneeId")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("ReporterId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ReporterId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClientCompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExecutorCompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.EmployeeProject", b =>
                {
                    b.HasOne("ProjectManagementApp.Domain.Entities.Employee", "Employee")
                        .WithMany("EmployeeProject")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProjectManagementApp.Domain.Entities.Project", "Project")
                        .WithMany("EmployeeProject")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.Issue", b =>
                {
                    b.HasOne("ProjectManagementApp.Domain.Entities.Employee", "Assignee")
                        .WithMany("AssignedIssues")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProjectManagementApp.Domain.Entities.Project", "Project")
                        .WithMany("Issues")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementApp.Domain.Entities.Employee", "Reporter")
                        .WithMany("ReportedIssues")
                        .HasForeignKey("ReporterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Project");

                    b.Navigation("Reporter");
                });

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.Project", b =>
                {
                    b.HasOne("ProjectManagementApp.Domain.Entities.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.Employee", b =>
                {
                    b.Navigation("AssignedIssues");

                    b.Navigation("EmployeeProject");

                    b.Navigation("ReportedIssues");
                });

            modelBuilder.Entity("ProjectManagementApp.Domain.Entities.Project", b =>
                {
                    b.Navigation("EmployeeProject");

                    b.Navigation("Issues");
                });
#pragma warning restore 612, 618
        }
    }
}
