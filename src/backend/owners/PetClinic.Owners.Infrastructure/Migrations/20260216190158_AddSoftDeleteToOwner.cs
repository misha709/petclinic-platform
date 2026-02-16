using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetClinic.Owners.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "owners",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_owners_deleted_at",
                table: "owners",
                column: "deleted_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_owners_deleted_at",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "owners");
        }
    }
}
