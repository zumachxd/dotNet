﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotNet.Migrations
{
    /// <inheritdoc />
    public partial class UserCharacterRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Userid",
                table: "Characters",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_Userid",
                table: "Characters",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_Userid",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_Userid",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "Characters");
        }
    }
}
