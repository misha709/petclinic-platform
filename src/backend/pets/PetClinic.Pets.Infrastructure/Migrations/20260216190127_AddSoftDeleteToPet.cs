using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetClinic.Pets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "pets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_pets_deleted_at",
                table: "pets",
                column: "deleted_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_pets_deleted_at",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "pets");
        }
    }
}
