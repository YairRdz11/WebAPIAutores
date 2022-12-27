using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIAutores.Migrations
{
    /// <inheritdoc />
    public partial class PublishBookDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Books",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Books");
        }
    }
}
