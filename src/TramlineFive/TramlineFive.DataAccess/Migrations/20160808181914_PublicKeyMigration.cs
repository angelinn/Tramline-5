using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TramlineFive.DataAccess.Migrations
{
    public partial class PublicKeyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Favourites_FavouriteID",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_FavouriteID",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "FavouriteID",
                table: "Stops");

            migrationBuilder.AddColumn<int>(
                name: "StopID",
                table: "Favourites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_StopID",
                table: "Favourites",
                column: "StopID");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Stops_StopID",
                table: "Favourites",
                column: "StopID",
                principalTable: "Stops",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Stops_StopID",
                table: "Favourites");

            migrationBuilder.DropIndex(
                name: "IX_Favourites_StopID",
                table: "Favourites");

            migrationBuilder.DropColumn(
                name: "StopID",
                table: "Favourites");

            migrationBuilder.AddColumn<int>(
                name: "FavouriteID",
                table: "Stops",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_FavouriteID",
                table: "Stops",
                column: "FavouriteID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Favourites_FavouriteID",
                table: "Stops",
                column: "FavouriteID",
                principalTable: "Favourites",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
