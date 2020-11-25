using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicinePlanner.Data.Migrations
{
    public partial class UpdatedFoodInterval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FoodInterval",
                table: "Medicines",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FoodInterval",
                table: "Medicines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
