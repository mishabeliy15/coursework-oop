using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace aeroflots.Migrations
{
    public partial class Ticket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Flights",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    OwnerId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Seats = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_TicketId",
                table: "Flights",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Tickets_TicketId",
                table: "Flights",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Tickets_TicketId",
                table: "Flights");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Flights_TicketId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Flights");
        }
    }
}
