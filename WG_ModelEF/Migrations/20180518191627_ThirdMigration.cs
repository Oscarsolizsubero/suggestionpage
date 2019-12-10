using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WG_ModelEF.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Topic_TopicVoterId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.RenameColumn(
                name: "TopicVoterId",
                table: "User",
                newName: "SuggestionVoterId");

            migrationBuilder.RenameIndex(
                name: "IX_User_TopicVoterId",
                table: "User",
                newName: "IX_User_SuggestionVoterId");

            migrationBuilder.CreateTable(
                name: "Suggestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    QuantityVote = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suggestion_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suggestion_AuthorId",
                table: "Suggestion",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Suggestion_SuggestionVoterId",
                table: "User",
                column: "SuggestionVoterId",
                principalTable: "Suggestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Suggestion_SuggestionVoterId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Suggestion");

            migrationBuilder.RenameColumn(
                name: "SuggestionVoterId",
                table: "User",
                newName: "TopicVoterId");

            migrationBuilder.RenameIndex(
                name: "IX_User_SuggestionVoterId",
                table: "User",
                newName: "IX_User_TopicVoterId");

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    QuantityVote = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topic_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topic_AuthorId",
                table: "Topic",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Topic_TopicVoterId",
                table: "User",
                column: "TopicVoterId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
