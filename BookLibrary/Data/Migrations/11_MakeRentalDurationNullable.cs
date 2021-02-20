using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookLibrary.Data.Migrations
{
    public partial class MakeRentalDurationNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RentalDuration",
                table: "Rentals",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RentalDuration",
                table: "Rentals",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
