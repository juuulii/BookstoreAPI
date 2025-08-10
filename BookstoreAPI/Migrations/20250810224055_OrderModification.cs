﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookstoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class OrderModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
