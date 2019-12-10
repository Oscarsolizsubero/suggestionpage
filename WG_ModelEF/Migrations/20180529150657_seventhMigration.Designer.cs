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
    [Migration("20180529150657_seventhMigration")]
    partial class seventhMigration
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

                    b.Property<int?>("AuthorId")
                        .IsRequired();

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

                    b.Property<int>("UserLvl");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WishGrid.Models.VotesUserSuggestion", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("SuggestionId");

                    b.HasKey("UserId", "SuggestionId");

                    b.HasIndex("SuggestionId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("WishGrid.Models.Suggestion", b =>
                {
                    b.HasOne("WishGrid.Models.User", "Author")
                        .WithMany("Suggestions")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WishGrid.Models.VotesUserSuggestion", b =>
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
