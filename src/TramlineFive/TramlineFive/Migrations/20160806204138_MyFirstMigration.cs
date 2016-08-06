using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TramlineFive.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    LiveTile = table.Column<bool>(nullable: false),
                    PushNotifications = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Directions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    LineID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Directions_Lines_LineID",
                        column: x => x.LineID,
                        principalTable: "Lines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    DirectionID = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Days_Directions_DirectionID",
                        column: x => x.DirectionID,
                        principalTable: "Directions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    DayID = table.Column<int>(nullable: true),
                    FavouriteID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TimingsAsString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stops_Days_DayID",
                        column: x => x.DayID,
                        principalTable: "Days",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stops_Favourites_FavouriteID",
                        column: x => x.FavouriteID,
                        principalTable: "Favourites",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Days_DirectionID",
                table: "Days",
                column: "DirectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Directions_LineID",
                table: "Directions",
                column: "LineID");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_DayID",
                table: "Stops",
                column: "DayID");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_FavouriteID",
                table: "Stops",
                column: "FavouriteID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.DropTable(
                name: "Directions");

            migrationBuilder.DropTable(
                name: "Lines");
        }
    }
}
