using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WG_ModelEF.Migrations
{
    public partial class seventhMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotesUserSuggestion_Suggestion_SuggestionId",
                table: "VotesUserSuggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_VotesUserSuggestion_User_UserId",
                table: "VotesUserSuggestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VotesUserSuggestion",
                table: "VotesUserSuggestion");

            migrationBuilder.RenameTable(
                name: "VotesUserSuggestion",
                newName: "Votes");

            migrationBuilder.RenameIndex(
                name: "IX_VotesUserSuggestion_SuggestionId",
                table: "Votes",
                newName: "IX_Votes_SuggestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                columns: new[] { "UserId", "SuggestionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Suggestion_SuggestionId",
                table: "Votes",
                column: "SuggestionId",
                principalTable: "Suggestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_User_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Suggestion_SuggestionId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_User_UserId",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.RenameTable(
                name: "Votes",
                newName: "VotesUserSuggestion");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_SuggestionId",
                table: "VotesUserSuggestion",
                newName: "IX_VotesUserSuggestion_SuggestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VotesUserSuggestion",
                table: "VotesUserSuggestion",
                columns: new[] { "UserId", "SuggestionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_VotesUserSuggestion_Suggestion_SuggestionId",
                table: "VotesUserSuggestion",
                column: "SuggestionId",
                principalTable: "Suggestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VotesUserSuggestion_User_UserId",
                table: "VotesUserSuggestion",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
