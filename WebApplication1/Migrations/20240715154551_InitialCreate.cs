using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionPlansComplete",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Monday = table.Column<int>(type: "INTEGER", nullable: false),
                    Tuesday = table.Column<int>(type: "INTEGER", nullable: false),
                    Wednesday = table.Column<int>(type: "INTEGER", nullable: false),
                    Thursday = table.Column<int>(type: "INTEGER", nullable: false),
                    Friday = table.Column<int>(type: "INTEGER", nullable: false),
                    Saturday = table.Column<int>(type: "INTEGER", nullable: false),
                    Sunday = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlansComplete", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductionPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    Monday = table.Column<int>(type: "INTEGER", nullable: false),
                    Tuesday = table.Column<int>(type: "INTEGER", nullable: false),
                    Wednesday = table.Column<int>(type: "INTEGER", nullable: false),
                    Thursday = table.Column<int>(type: "INTEGER", nullable: false),
                    Friday = table.Column<int>(type: "INTEGER", nullable: false),
                    Saturday = table.Column<int>(type: "INTEGER", nullable: false),
                    Sunday = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionPlansComplete");

            migrationBuilder.DropTable(
                name: "ProductionResults");
        }
    }
}
