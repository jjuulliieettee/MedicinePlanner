using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicinePlanner.Data.Migrations
{
    public partial class ChangedFoodRelationPharmaceuticalFormMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medicines_FoodRelationId",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_PharmaceuticalFormId",
                table: "Medicines");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_FoodRelationId",
                table: "Medicines",
                column: "FoodRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_PharmaceuticalFormId",
                table: "Medicines",
                column: "PharmaceuticalFormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medicines_FoodRelationId",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_PharmaceuticalFormId",
                table: "Medicines");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_FoodRelationId",
                table: "Medicines",
                column: "FoodRelationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_PharmaceuticalFormId",
                table: "Medicines",
                column: "PharmaceuticalFormId",
                unique: true);
        }
    }
}
