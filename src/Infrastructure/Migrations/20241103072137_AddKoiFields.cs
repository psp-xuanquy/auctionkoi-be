using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKoiFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DescendingRate",
                table: "Koi",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DescendingTimeInMinute",
                table: "Koi",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LowestDescendedPrice",
                table: "Koi",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescendingRate",
                table: "Koi");

            migrationBuilder.DropColumn(
                name: "DescendingTimeInMinute",
                table: "Koi");

            migrationBuilder.DropColumn(
                name: "LowestDescendedPrice",
                table: "Koi");
        }
    }
}
