using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WG_ModelEF.Migrations
{
    public partial class FifthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Suggestion_SuggestionVoterId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_SuggestionVoterId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SuggestionVoterId",
                table: "User");

            migrationBuilder.CreateTable(
                name: "VotesUserSuggestion",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    SuggestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotesUserSuggestion", x => new { x.UserId, x.SuggestionId });
                    table.ForeignKey(
                        name: "FK_VotesUserSuggestion_Suggestion_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "Suggestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VotesUserSuggestion_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VotesUserSuggestion_SuggestionId",
                table: "VotesUserSuggestion",
                column: "SuggestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotesUserSuggestion");

            migrationBuilder.AddColumn<int>(
                name: "SuggestionVoterId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_SuggestionVoterId",
                table: "User",
                column: "SuggestionVoterId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Suggestion_SuggestionVoterId",
                table: "User",
                column: "SuggestionVoterId",
                principalTable: "Suggestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
