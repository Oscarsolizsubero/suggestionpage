﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WishGrid.Models;

namespace WG_ModelEF.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180809151647_SixteenthMigration")]
    partial class SixteenthMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WishGrid.Models.Reply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorId")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int?>("SuggestionId")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("SuggestionId");

                    b.ToTable("Replies");
                });

            modelBuilder.Entity("WishGrid.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("WishGrid.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("WishGrid.Models.Suggestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("QuantityVote");

                    b.Property<int>("StatusId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("StatusId");

                    b.ToTable("Suggestion");
                });

            modelBuilder.Entity("WishGrid.Models.Tenants", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<bool>("Moderation");

                    b.Property<string>("NameTenants")
                        .IsRequired();

                    b.Property<string>("Nit");

                    b.Property<string>("Phone");

                    b.Property<bool>("Status");

                    b.Property<string>("URLImage");

                    b.Property<string>("URLOrigin");

                    b.Property<string>("URLTenant");

                    b.HasKey("Id");

                    b.ToTable("Tenant");
                });

            modelBuilder.Entity("WishGrid.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("LastName");

                    b.Property<bool>("Validation");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<int>("RoleId");

                    b.Property<int>("TenantId");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("TenantId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WishGrid.Models.Votes", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("SuggestionId");

                    b.HasKey("UserId", "SuggestionId");

                    b.HasIndex("SuggestionId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("WishGrid.Models.Reply", b =>
                {
                    b.HasOne("WishGrid.Models.User", "Author")
                        .WithMany("Replies")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WishGrid.Models.Suggestion", "Suggestion")
                        .WithMany("Replies")
                        .HasForeignKey("SuggestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WishGrid.Models.Suggestion", b =>
                {
                    b.HasOne("WishGrid.Models.User", "Author")
                        .WithMany("Suggestions")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WishGrid.Models.Status", "Status")
                        .WithOne("Suggestion")
                        .HasForeignKey("WishGrid.Models.Suggestion", "StatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WishGrid.Models.User", b =>
                {
                    b.HasOne("WishGrid.Models.Role", "Role")
                        .WithOne("User")
                        .HasForeignKey("WishGrid.Models.User", "RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WishGrid.Models.Tenants", "Tenant")
                        .WithOne("User")
                        .HasForeignKey("WishGrid.Models.User", "TenantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WishGrid.Models.Votes", b =>
                {
                    b.HasOne("WishGrid.Models.Suggestion", "Suggestion")
                        .WithMany("VotesUserSuggestions")
                        .HasForeignKey("SuggestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WishGrid.Models.User", "User")
                        .WithMany("VotesUserSuggestions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
