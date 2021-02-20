using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookLibrary.Data.Migrations
{
    public partial class SeedRentals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"


SET DATEFORMAT dmy;

DECLARE @date_from_rent DATETIME;
DECLARE @date_from_return DATETIME;
DECLARE @date_to_rent DATETIME;
DECLARE @date_to_return DATETIME;


SET @date_from_rent = '01.01.1950';
SET @date_to_rent = '01.01.2000';

SET @date_from_return = '02.01.2000'
SET @date_to_return = '01.01.2018';

DECLARE @date_rent DATETIME;
DECLARE @date_return DATETIME;
DECLARE @duration INT;

DECLARE @i int = 0

WHILE @i < 1000
BEGIN
    SET @i = @i + 1
    /* do some work */

	DECLARE @book_id UNIQUEIDENTIFIER;
	SELECT TOP 1 @book_id=Id FROM Books ORDER BY NEWID()

	DECLARE @client_id UNIQUEIDENTIFIER;
	SELECT TOP 1 @client_id=Id FROM Clients as c
	WHERE c.Id NOT IN (SELECT ClientId FROM Rentals) OR @book_id NOT IN (SELECT BookId FROM Rentals WHERE ClientId=c.Id)
	ORDER BY NEWID()


	SET @date_rent = @date_from_rent + (ABS(CAST(CAST(NewID() AS BINARY(8)) AS INT)) % CAST((@date_to_rent - @date_from_rent) AS INT));
	SET @date_return = @date_from_return + (ABS(CAST(CAST(NewID() AS BINARY(8)) AS INT)) % CAST((@date_to_return - @date_from_return) AS INT));

	SET @duration = 7 + CAST(RAND() * 10000 as INT) % 203;

	INSERT INTO [dbo].[Rentals] ([Id], [BookId], [ClientId], [RentalDate], [RentalDuration], [ReturnDate]) VALUES (NewId(), @book_id, @client_id, @date_rent, @duration, @date_return)
END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Rentals");
        }
    }
}
