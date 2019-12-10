﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WishGrid.Models;

namespace WG_ModelEF.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180518191627_ThirdMigration")]
    partial class ThirdMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WishGrid.Models.Suggestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AuthorId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("QuantityVote");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Suggestion");
                });

            modelBuilder.Entity("WishGrid.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<int?>("SuggestionVoterId");

                    b.Property<string>("UserLvl");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SuggestionVoterId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WishGrid.Models.Suggestion", b =>
                {
                    b.HasOne("WishGrid.Models.User", "Author")
                        .WithMany("Suggestions")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("WishGrid.Models.User", b =>
                {
                    b.HasOne("WishGrid.Models.Suggestion", "SuggestionVoter")
                        .WithMany("Voters")
                        .HasForeignKey("SuggestionVoterId");
                });
#pragma warning restore 612, 618
        }
    }
}