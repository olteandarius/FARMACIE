using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookLibrary.Data.Migrations
{
    public partial class SeedAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // admin user / pass: darius@darius.com / Huawei12# 
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName]) VALUES (N'3efd7f2f-3c2f-495b-885c-8414eb0ee02e', 0, N'cd03333f-91c5-46f5-8549-dfa16d251034', N'darius@darius.com', 0, 1, NULL, N'darius@darius.com', N'darius@darius.com', N'AQAAAAEAACcQAAAAEBmn9CHnr3a5vHGzanghM60LPz96m+X1zHyOSSrTTqQoN/He/r+IvNUu3cwhfW7sqQ==', NULL, 0, N'e500a4e5-872f-4bf4-976d-dd0c20b6f395', 0, N'darius@darius.com')");
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES (N'6894a796-779f-4827-bf23-45925e778865', NULL, N'Admin', NULL)");
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3efd7f2f-3c2f-495b-885c-8414eb0ee02e', N'6894a796-779f-4827-bf23-45925e778865')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUserRoles] where [UserId]=N'3efd7f2f-3c2f-495b-885c-8414eb0ee02e' and [RoleId]=N'6894a796-779f-4827-bf23-45925e778865')");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] where [Id]=N'6894a796-779f-4827-bf23-45925e778865'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] where [Id]=N'3efd7f2f-3c2f-495b-885c-8414eb0ee02e'");
        }
    }
}
