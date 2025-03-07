using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedisExercise.Migrations
{
    /// <inheritdoc />
    public partial class mig_second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fiyat",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Adi",
                table: "Products",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "Fiyat");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "Adi");
        }
    }
}
