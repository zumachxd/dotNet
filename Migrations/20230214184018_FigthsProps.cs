using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotNet.Migrations
{
    /// <inheritdoc />
    public partial class FigthsProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Defeats",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Figths",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Victories",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defeats",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Figths",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Victories",
                table: "Characters");
        }
    }
}
