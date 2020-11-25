using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicinePlanner.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodRelations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PharmaceuticalForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaceuticalForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ActiveSubstance = table.Column<string>(nullable: false),
                    Dosage = table.Column<int>(nullable: false),
                    NumberOfTakes = table.Column<int>(nullable: false),
                    FoodInterval = table.Column<int>(nullable: true),
                    PharmaceuticalFormId = table.Column<int>(nullable: false),
                    FoodRelationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_FoodRelations_FoodRelationId",
                        column: x => x.FoodRelationId,
                        principalTable: "FoodRelations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medicines_PharmaceuticalForms_PharmaceuticalFormId",
                        column: x => x.PharmaceuticalFormId,
                        principalTable: "PharmaceuticalForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineSchedules_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicineSchedules_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    TimeOfFirstMeal = table.Column<DateTimeOffset>(nullable: false),
                    NumberOfMeals = table.Column<int>(nullable: false),
                    MedicineScheduleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodSchedules_MedicineSchedules_MedicineScheduleId",
                        column: x => x.MedicineScheduleId,
                        principalTable: "MedicineSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodSchedules_MedicineScheduleId",
                table: "FoodSchedules",
                column: "MedicineScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_FoodRelationId",
                table: "Medicines",
                column: "FoodRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_PharmaceuticalFormId",
                table: "Medicines",
                column: "PharmaceuticalFormId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineSchedules_MedicineId",
                table: "MedicineSchedules",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineSchedules_UserId",
                table: "MedicineSchedules",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodSchedules");

            migrationBuilder.DropTable(
                name: "MedicineSchedules");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FoodRelations");

            migrationBuilder.DropTable(
                name: "PharmaceuticalForms");
        }
    }
}
