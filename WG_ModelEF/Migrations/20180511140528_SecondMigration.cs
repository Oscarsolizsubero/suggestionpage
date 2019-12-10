using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WG_ModelEF.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TopicVoterId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_TopicVoterId",
                table: "User",
                column: "TopicVoterId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Topic_TopicVoterId",
                table: "User",
                column: "TopicVoterId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Topic_TopicVoterId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_TopicVoterId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TopicVoterId",
                table: "User");
        }
    }
}
