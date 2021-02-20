using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookLibrary.Data.Migrations
{
    public partial class AddClientOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_OwnerId",
                table: "Clients",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AspNetUsers_OwnerId",
                table: "Clients",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AspNetUsers_OwnerId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_OwnerId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Clients");
        }
    }
}
