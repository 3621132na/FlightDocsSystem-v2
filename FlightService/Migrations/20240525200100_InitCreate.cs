using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightService.Migrations
{
    public partial class InitCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AircraftType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SentFiles = table.Column<int>(type: "int", nullable: false),
                    ReturnedFiles = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DepartureAirportID = table.Column<int>(type: "int", nullable: false),
                    DepartureAirportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalAirportID = table.Column<int>(type: "int", nullable: false),
                    ArrivalAirportName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: true),
                    FlightID = table.Column<int>(type: "int", nullable: true),
                    FlightID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_User_Flights_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flights",
                        principalColumn: "FlightID");
                    table.ForeignKey(
                        name: "FK_User_Flights_FlightID1",
                        column: x => x.FlightID1,
                        principalTable: "Flights",
                        principalColumn: "FlightID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_FlightID",
                table: "User",
                column: "FlightID");

            migrationBuilder.CreateIndex(
                name: "IX_User_FlightID1",
                table: "User",
                column: "FlightID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
