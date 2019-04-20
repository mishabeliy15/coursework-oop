using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace aeroflots.Migrations
{
    public partial class FixedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightShedules");

            migrationBuilder.CreateTable(
                name: "FlightSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Class = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Departure = table.Column<string>(nullable: false),
                    DepartureTime = table.Column<TimeSpan>(nullable: false),
                    Arrival = table.Column<string>(nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(nullable: false),
                    Seats = table.Column<int>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    Cost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSchedules", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightSchedules");

            migrationBuilder.CreateTable(
                name: "FlightShedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Arrival = table.Column<string>(nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    Class = table.Column<int>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Departure = table.Column<string>(nullable: false),
                    DepartureTime = table.Column<TimeSpan>(nullable: false),
                    Seats = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightShedules", x => x.Id);
                });
        }
    }
}
